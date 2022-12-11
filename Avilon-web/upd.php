<?php
    function Update_CarList($conn)
    {
        return $Equipments = $conn->query("Select * from Equipments");
    }
?>