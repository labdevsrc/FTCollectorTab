<?php
	include "conn.php";
	
	$sql="select id, crew_leader, team_member, created_on from crew_default";
	$res= mysqli_query($con,$sql);	
	$data = array();
	while($row = mysqli_fetch_assoc($res)){
		$data[] = $row;
	}
    
	echo json_encode($data);

?>
