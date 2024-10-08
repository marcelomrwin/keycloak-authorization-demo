# Keycloak Authorization Services Demo with Dotnet Core 8

This project demonstrates **Keycloak Authorization Services** using **RBAC (Role-Based Access Control)** and **ABAC (Attribute-Based Access Control)** models.

[Keycloak](https://keycloak.org)
[Authorization services guide](https://www.keycloak.org/docs/24.0.5/authorization_services)

The examples were implemented using [.Net Core](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet) 8 and its integration with JWT.

## What are RBAC and ABAC?

- **RBAC (Role-Based Access Control)**: Access control based on user roles, such as "Admin," "User," or "Editor." Useful in environments where access can be managed through well-defined roles.
- **ABAC (Attribute-Based Access Control)**: Access control based on user, resource, or environment attributes, like location or department. ABAC is more dynamic and context-sensitive than RBAC, offering greater flexibility.

## Project Structure

### RBAC Demo

The **RBAC** (Role-Based Access Control) demo illustrates access control for specific resources based on user roles. Contains **.NET Core 8** and **Node.js with Express** implementations for RBAC and Demonstrates how to use Keycloak authorization services focusing on role-based access.

In this scenario, we've defined three resources:

- `admin_resource`
- `editor_resource`
- `viewer_resource`

Each resource allows three operations:

- `edit`
- `view`
- `delete`

The `.NET` project in `rbac/dotnet` demonstrates how access to `admin_resource` is restricted to users with the `admin` role. Only the `admin_user` has this role assigned.

#### Testing Instructions

To test this setup, use the Postman collection:

1. Log in with any user except `admin_user`. There are three users in the RBAC realm:

   - `admin_user`
   - `editor_user`
   - `viewer_user`

   All passwords are set to `password`.

2. After logging in with a user other than `admin_user`, try accessing the `/admin` or `/admin/authz` endpoints. These endpoints will deny access due to insufficient authorization.

3. Log in as `admin_user` to successfully access these endpoints.

This demo shows how RBAC ensures that only users with the appropriate role (`admin` in this case) can access specific resources.


### ABAC Demo

The **ABAC** (Attribute-Based Access Control) demo showcases access control based on user attributes. Includes a **.NET Core 8** project and a **Keycloak JS Policy**. The ABAC example uses JavaScript rules to evaluate dynamic attributes, based on the [JavaScript Provider documentation](https://www.keycloak.org/docs/24.0.5/server_development/#_script_providers)

This example includes two resources:

- `Cooper Mine Operations - Peru`
- `Oil Platform - Australia`

The operations available for both resources are the same, but access is determined by specific user attributes. 

#### User and Attribute Requirements

There are two users in the ABAC realm:

- `operations_user`
- `oil_user`

Each has the password `password`. Access to the resources depends on the user's attributes, as follows:

| Resource                     | Required Department | Required Location |
|------------------------------|---------------------|-------------------|
| Cooper Mine Operations - Peru | Operations         | Peru              |
| Oil Platform - Australia      | Oil                | Australia         |

Only users with the correct department and location attributes can access each respective resource.

#### User Groups for Attribute Management

To streamline attribute management, two groups were created:

- `Cooper Mine Peru` – Members have the attributes `department` set to **Operations** and `location` set to **Peru**. Members of this group can access the `Cooper Mine Operations - Peru` resource.
- `Oil Australia` – Members have the attributes `department` set to **Oil** and `location` set to **Australia**. Members of this group can access the `Oil Platform - Australia` resource.

#### Testing Instructions

To test this setup, use the Postman collection:

1. Log in with either user and try accessing the `/oil` and `/operations` endpoints.
2. You will see that each user can only access the resource directly linked to their attributes.

This example demonstrates how ABAC dynamically manages access based on user-specific attributes, offering more contextual control than RBAC.

### config/: 
  - Realm configuration file for Keycloak import.

### docker-compose.yaml: 
  - Sets up a test environment with **PostgreSQL** and **Keycloak 24**, automatically importing the example realm.

## Setup and Running

1. **Prerequisites**:
   - Install Docker, Docker Compose, .NET Core SDK 8, and Node.js.

2. **Setup**:
   - Clone the repo:
     ```shell
     git clone https://github.com/marcelomrwin/keycloak-authorization-demo.git
     cd keycloak-authorization-demo
     ```

3. **Start the Environment**:
   - Use Docker Compose to start Keycloak and PostgreSQL:
     ```shell
     docker-compose up -d
     ```

4. **Keycloak Configuration**:
   - Keycloak will start with the imported realm from the `config` folder.

5. **Running the Examples**:
   - **RBAC**:
     - Go to the `rbac` folder and run either the .NET or Node.js project:
       ```shell
       cd rbac/dotnet
       dotnet run
       # or for Node.js
       cd rbac/nodejs
       npm install
       node server.js
       ```

   - **ABAC**:
     - Go to the `abac` folder and run the .NET Core project, which requires custom JavaScript policies for Keycloak:
       ```shell
       cd abac/dotnet
       dotnet run
       ```
     - JavaScript policy files for ABAC are in `abac/keycloak-js-policy`.

6. **Generating the Keycloak JS Policy JAR**:
   - To package the JavaScript policy as a `.jar`, run the following from the `abac` directory:
     ```shell
     cd abac
     jar cvf keycloak-js-policy.jar -C keycloak-js-policy .
     ```

7. **Testing the Setup**:
   - Access the Keycloak admin console (http://localhost:8180) and test the imported realms with predefined RBAC and ABAC roles.
   - Use the **Evaluate** tab in the Keycloak admin console to simulate different attribute scenarios for ABAC.

8. **Postman Collection and Environments**

In the `postman` folder, you'll find the **Postman collection** and **environments** to test the RBAC and ABAC examples:

8.1. **Import the Collection**:
   - Open Postman, go to **File > Import**, and select `Authz_Keycloak.postman_collection.json` from the `postman` folder.

8.2. **Environments**:
   - There are two environments: `Keycloak_authz_rbac_localhost.json` and `Keycloak_authz_abac_localhost.json`.
   - Import each environment by navigating to **File > Import** and selecting the environment files.
   - Choose the appropriate environment from the dropdown in the top-right corner of Postman to switch between `rbac` and `abac` realms.


## Additional Notes
- **JavaScript Policy**: The ABAC demo requires custom JavaScript rules, following the structure outlined in the [JavaScript Provider documentation](https://www.keycloak.org/docs/24.0.5/server_development/#_script_providers).
