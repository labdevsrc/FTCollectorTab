<?php
    // echo "test val-".$_POST['evtype'];
    // print_r($_POST['evtype']);
     header('Content-type: application/json');
     $json = array("status" => 0, "msg" => $_POST['evtype']);
	echo json_encode($json);
?>