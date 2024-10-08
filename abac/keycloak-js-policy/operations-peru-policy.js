var context = $evaluation.getContext();
var identity = context.getIdentity();
var attributes = identity.getAttributes();

var department = attributes.getValue('department') ? attributes.getValue('department').asString(0) : null;
var location = attributes.getValue('location') ? attributes.getValue('location').asString(0) : null;

if (department && location) {    

    // Restrict access to users in the "Operations" department and location "Peru"
    if (department === 'Operations' && location === 'Peru') {
        $evaluation.grant();
    }
}