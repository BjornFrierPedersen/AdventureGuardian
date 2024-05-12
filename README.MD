Running the application with locally hosted postgres and keycloak in docker

1. Ensure you have docker-desktop installed
2. Navigate to the root folder of the solution file
3. Run the command: docker-compose up -d
4. Go to url: http://localhost:28080/admin/master/console/
5. Click on the dropdown box in the far left corner labeled keycloak
6. Click on create realm
7. Drag and drop the file .\infrastructure-configuration\keycloak-realm-export.json into the resource file textbox
8. Go to Users
	8.1. Click on the 'Add user' button
	8.2. Input the required fields and click on next
	8.3. Click on the 'Role mapping' banner 
	8.4. Click on the 'Assign role' button and select the 'adventureguardian' role, which is the base role
	8.5. Click on the "Credentials" banner and click the 'Set password' button
	8.6. Input password and set Temporary to 'Off'
9. Start the AdventureGuardian application
10. Goto url: http://localhost:5058/swagger/index.html
11. Click on Get '/help/token/get and input username and password you just created
12. Copy token and goto the top of the page and click on the 'Authorize' button
13. Input 'Bearer {token}' and click on 'Authorize'
14. Start making requests	
