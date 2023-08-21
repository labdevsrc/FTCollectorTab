<?php
	$server="myfibertrak.crrkc3tsjnuq.us-east-2.rds.amazonaws.com";
	$username="Rajib.Banerjee";
	$pass="95Rbsps5mvZ9T8Wm";
	//$database="field_data"; 
	$database="backup_of_myfibertrak";
	//$database="myfibertrak";
	$con=mysqli_connect($server,$username,$pass,$database);
	//echo "Connected";
	if (!$con) {
    die("Database Connection failed: " . mysqli_connect_error());
    }
	
	$sql="select email, password, first_name, last_name, created_on from end_user where record_state='L' and field_data_collection='Y'";
	$res= mysqli_query($con,$sql);	
	$data = array();
	while($row = mysqli_fetch_assoc($res)){
		$data[] = $row;
	}


	echo json_encode($data);

?>
