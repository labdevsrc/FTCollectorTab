<?php
	include "conn.php";
	
	$sql="select email, end_user.key as UserKey, password, first_name, last_name, created_on from end_user where record_state='L' and field_data_collection='Y'";
	$res= mysqli_query($con,$sql);	
	$data = array();
	while($row = mysqli_fetch_assoc($res)){
		$data[] = $row;
	}


	echo json_encode($data);

?>
