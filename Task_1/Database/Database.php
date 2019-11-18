<?php
//Создан для благих целей

class Database
{
    private $using;

    public static function new($name) {
        $db = new Database();
        $db -> using = $name.".db";

        $xml = simplexml_load_string("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><database><name>$name</name><tables></tables></database>");
        $xml -> asXML($name.".db");

        return $db;
    }

    public static function load($name) {
        $db = new Database();
        $db -> using = $name.".db";

        return $db;
    }

    public function addTable($name, $fields) {
        $xml = simplexml_load_file( $this -> using);

        $val = $this->findTable($xml, $name);
        if ($val != null)
            return "Table exists";

        $table = $xml -> tables -> addChild("table");
        $nameXml = $table -> addChild("name");
        $nameXml[0] = $name;
        $fieldsXml = $table -> addChild("fields");
        foreach ($fields as $field) {
            $fieldXml = $fieldsXml -> addChild("field");
            $fieldXml[0] = $field;
        }
        $aiXml = $table -> addChild("auto_increment_value");
        $aiXml[0] = 0;
        $table -> addChild("rows");

        $xml -> asXML($this -> using);

        return "Table created";
    }

    public function insert($tableName, $values) {
        $xml = simplexml_load_file($this -> using);
        $table = $this -> findTable($xml, $tableName);

        $table -> row_count = $table -> row_count + 1;
        $row = $table -> rows -> addChild("row");

        for ($i = 0; $i < $table -> fields -> field -> count(); $i++) {
            $fieldName = $table -> fields -> field[$i];
            $value = $values[$i];
            $field = $row -> addChild($fieldName);
            if ($value == "[AUTO_INCREMENT]") {
                $table -> auto_increment_value = $table -> auto_increment_value + 1;
                $field[0] = $table -> auto_increment_value;
            }
            else {
                $field[0] = $value;
            }
        }

        $xml -> asXML($this -> using);

        return $row;
    }

    public function update($tableName, $where, $values) {
        $xml = simplexml_load_file($this -> using);
        $table = $this -> findTable($xml, $tableName);

        $rows = $this -> findRows($table, $where);
        foreach ($rows as $row) {
            for ($i = 0; $i < $table -> fields -> field -> count(); $i++) {
                if ($values[$i] == "[NOT_UPDATED]")
                    continue;
                $row -> { $table -> fields -> field[$i] } = $values[$i];
            }
        }

        $xml -> asXML($this -> using);
        return $rows;
    }

    public function delete($tableName, $where) {
        $xml = simplexml_load_file($this -> using);
        $table = $this -> findTable($xml, $tableName);

        $rows = $this -> findRows($table, $where);
        foreach ($rows as $row) {
            $dom=dom_import_simplexml($row);
            $dom->parentNode->removeChild($dom);
        }

        $xml -> asXML($this -> using);

        return $rows;
    }

    public function select($tableName, $where) {
        $xml = simplexml_load_file($this -> using);
        $table = $this -> findTable($xml, $tableName);

        return $this -> findRows($table, $where);
    }

    private function findTable($xml, $name) {
        foreach ($xml -> tables -> table as $table) {
            if ($table -> name == $name)
                return $table;
        }
        return null;
    }

    private function findRows($table, $where) {
        $rows = [];
		error_reporting(E_ERROR | E_PARSE);
        foreach ($table -> rows -> row as $row) {
            foreach ($table -> fields -> field as $field) {
                if ($where != "[ALL]") {
                    foreach ($where as $cond) {
                        $w = str_replace("[$field]", "\$row -> $field", $cond);
                        if (eval("error_reporting( error_reporting() & ~E_NOTICE ); return " . $w . ";")) {
                            array_push($rows, $row);
                        }
                    }
                }
                else
                    array_push($rows, $row);
            }
        }

        $result = array();
        foreach ($rows as $key => $value){
            if(!in_array($value, $result))
                $result[$key]=$value;
        }

        return $result;
    }
}