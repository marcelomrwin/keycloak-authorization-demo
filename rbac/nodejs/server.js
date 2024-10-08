const express = require('express');
const session = require('express-session');
const Keycloak = require('keycloak-connect');
const bodyParser = require('body-parser');
const axios = require('axios');

const app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

const memoryStore = new session.MemoryStore();

app.use(session({
    secret: 'some secret',
    resave: false,
    saveUninitialized: true,
    store: memoryStore
}));

const keycloak = new Keycloak({ store: memoryStore });

app.use(keycloak.middleware());

async function checkPermission(token, permission, scope) {
    try {
        const response = await axios.post('http://localhost:8180/realms/rbac/protocol/openid-connect/token', {
            grant_type: 'urn:ietf:params:oauth:grant-type:uma-ticket',
            response_mode: 'decision',
            permission: permission+'#'+scope,
            claim_token_format: 'urn:ietf:params:oauth:token-type:jwt',
            audience: 'rbac_client'
        }, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': `Bearer ${token}`
            }
        });

        return response.data.result === true;
    } catch (error) {
        if (error.response && error.response.status === 403){
            return false; // Access denied
        }
        console.error('Error checking permissions:', error.response ? error.response.data : error.message);
        return false;
    }
}


// admin using token
app.get('/admin', keycloak.protect('realm:admin'), (req, res) => {
    res.send("Access granted to administrators");
});

//admin using authorization server
app.get('/admin/authz', keycloak.protect(), async (req, res) => {
    const token = req.kauth.grant.access_token.token;
    const hasPermission = await checkPermission(token, 'admin_resource', 'edit');

    if (hasPermission) {
        res.send("Access granted by the Authorization Server");
    } else {
        res.status(403).send("Access Denied");
    }
});


app.get('/public', (req, res) => {
    res.send("Public Access");
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
