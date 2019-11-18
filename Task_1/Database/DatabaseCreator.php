<?php
require "Database.php";

//Creating database
$db = Database::new("Project");

//Creating tables
$db -> addTable("users", ["id", "login", "password", "role", "name", "secondName", "lastTimeOnline", "status"]);

//Creating admin
$db -> insert("users", ["[AUTO_INCREMENT]", "admin", "admin", "admin", "Ruslan", "Kudrjavtsev", "5 Nov 2019 17:13", "OK"]);

header("LOCATION: ../index.php");