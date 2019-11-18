<?php
$action = null;
$statusSort = "OK";

if (isset($_GET["statusSort"]))
	$statusSort = $_GET["statusSort"];

if (isset($_POST["action"]))
    $action = $_POST["action"];
else if (isset($_GET["action"]))
    $action = $_GET["action"];

if (!empty($action)) {
    if ($action == "insert") {
        $id = "[AUTO_INCREMENT]";
        $login = $_POST["login"];
        $password = $_POST["password"];
        $role = $_POST["role"];
        $name = $_POST["name"];
        $secondName = $_POST["secondName"];
        $date = date('m/d/Y h:i:s a', time());
        $status = $_POST["status"];
        if ($status) {
            $status = "OK";
        }
        else {
            $status = "NOT ACTIVATED";
        }

        $db -> insert("users", [$id, $login, $password, $role, $name, $secondName, $date, $status]);
        header("location: index.php?statusSort=$statusSort");
    }
    else if ($action == "delete") {
        $id = $_GET["id"];
        $db -> delete("users", ["[id]==$id"]);
        header("location: index.php?statusSort=$statusSort");
    }
    else if ($action == "update" && isset($_POST["id"])) {
        $id = $_POST["id"];
        $login = $_POST["login"];
        $password = $_POST["password"];
        $role = $_POST["role"];
        $name = $_POST["name"];
        $secondName = $_POST["secondName"];
        $lastOnline = date('m/d/Y h:i:s a', time());
        $status = "[NOT_UPDATED]";
        if (isset($_POST["status"])) {
            $status = $_POST["status"];
            if ($status)
                $status = "OK";
            else if (!$status)
                $status = "NOT ACTIVATED";
        }
        else {
            $status = "NOT ACTIVATED";
        }

        $db -> update("users", ["[id]==$id"], ["[NOT_UPDATED]", "$login", "$password", "$role", "$name", "$secondName", "$lastOnline", "$status"]);
		header("location: index.php?statusSort=$statusSort");
    }
}

?>
<div>
    <table border="1">
        <tr>
            <th>id</th>
            <th>login</th>
            <th>password</th>
            <th>role</th>
            <th>name</th>
            <th>second name</th>
            <th>last time online</th>
            <th><a href="?statusSort=<?php if ($statusSort == "OK") echo "NOT ACTIVATED"; else echo "OK";?>">status</a></th>
        </tr>
        <?php
        $rows = $db -> select("users", "[ALL]");
		$toPrint = [];
		foreach($rows as $row) {
			if ($row -> status == $statusSort)
				array_push($toPrint, $row);
		}
		
		foreach($rows as $row) {
			if ($row -> status != $statusSort)
				array_push($toPrint, $row);
		}
		
        foreach ($toPrint as $row) {
            echo "<tr>";
            echo "<td>".($row -> id)."</td>";
            echo "<td>".($row -> login)."</td>";
            echo "<td>".($row -> password)."</td>";
            echo "<td>".($row -> role)."</td>";
            echo "<td>".($row -> name)."</td>";
            echo "<td>".($row -> secondName)."</td>";
            echo "<td>".($row -> lastTimeOnline)."</td>";
            echo "<td>".($row -> status)."</td>";
            echo "<td><a href='?action=update&id=".($row -> id)."&statusSort=$statusSort'>Edit</a></td>";
            echo "<td><a href='?action=delete&id=".($row -> id)."&statusSort=$statusSort'>Delete</a></td>";
            echo "</tr>";
        }
        ?>
    </table>
    <hr>
    <?php
    if (!isset($_GET["id"])) {
    ?>
    <form method="post" action="?statusSort=<?=$statusSort?>">
        <input type="hidden" name="action" value="insert">
        <input type="text" name="login" placeholder="login"> <br>
        <input type="text" name="password" placeholder="password"> <br>
        <input type="text" name="role" placeholder="role"> <br>
        <input type="text" name="name" placeholder="name"> <br>
        <input type="text" name="secondName" placeholder="second name"> <br>
        <input type="checkbox" name="status" id="statusBox"><label for="statusBox">Is activated</label> <br>
        <input type="submit" value="Insert">
    </form>
    <?php }
    else {
        $row = $db -> select("users", ["[id]==".$_GET["id"]])[0];
        $status = $row -> status;
        echo "<form method=\"post\" action=\"?statusSort=".$statusSort."\">";
        echo '<input type="hidden" name="action" value="update">';
        echo '<input type="hidden" name="id" value="'.($_GET["id"]).'">';
        echo '<input type="text" name="login" placeholder="login" value="'.($row -> login).'"> <br>';
        echo '<input type="text" name="password" placeholder="password" value="'.($row -> password).'"> <br>';
        echo '<input type="text" name="role" placeholder="role" value="'.($row -> role).'"> <br>';
        echo '<input type="text" name="name" placeholder="name" value="'.($row -> name).'"> <br>';
        echo '<input type="text" name="secondName" placeholder="second name" value="'.($row -> secondName).'"> <br>';
        if ($status == "OK")
            echo '<input type="checkbox" name="status" id="statusBox" checked><label for="statusBox">Is activated</label> <br>';
        else
            echo '<input type="checkbox" name="status" id="statusBox"><label for="statusBox">Is activated</label> <br>';
        echo '<input type="submit" value="Update">';
        echo '</form>';
    } ?>
</div>
