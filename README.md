# Keycloak Authorization Services Demo

This project demonstrates **Keycloak Authorization Services** using **RBAC (Role-Based Access Control)** and **ABAC (Attribute-Based Access Control)** models.

[Authorization services guide](https://www.keycloak.org/docs/24.0.5/authorization_services)

## What are RBAC and ABAC?

- **RBAC (Role-Based Access Control)**: Access control based on user roles, such as "Admin," "User," or "Editor." Useful in environments where access can be managed through well-defined roles.
- **ABAC (Attribute-Based Access Control)**: Access control based on user, resource, or environment attributes, like location or department. ABAC is more dynamic and context-sensitive than RBAC, offering greater flexibility.

## Project Structure

- `rbac/`: 
  - Contains **.NET Core 8** and **Node.js with Express** implementations for RBAC.
  - Demonstrates how to use Keycloak authorization services focusing on role-based access.

- `abac/`: 
  - Includes a **.NET Core 8** project and a **Keycloak JS Policy**.
  - The ABAC example uses JavaScript rules to evaluate dynamic attributes, based on the [JavaScript Provider documentation](https://www.keycloak.org/docs/24.0.5/server_development/#_script_providers).

- `config/`: 
  - Realm configuration file for Keycloak import.

- `docker-compose.yaml`: 
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
