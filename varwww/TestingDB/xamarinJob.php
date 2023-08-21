<?php 
	include "conn.php";
	$sql="select Job.id, Job.key as JobKey, Job.number as JobNumber, location as JobLocation, Job.owner_key as OwnerKey, Job.OWNER_CD , Job.county_code as CountyCode, Own.name as OwnerName, Cust.name as CustomerName, Job.contact_phone as CustomerPhone,
	stage, show_all as ShowAll, Job.phases as JobPhases,
	contact_name as ContactName from Job 
left outer join (select * from myfibertrak.owner where owner.record_state = 'L') as Own on Own.key = Job.owner_key
left outer join (select * from Customer where Customer.record_state = 'L') as Cust on Cust.key = Job.customer
where Job.record_state='L' ";

	$res= mysqli_query($con,$sql);	
	$data = array();
	while($row = mysqli_fetch_assoc($res)){
		$data[] = $row;
	}


	echo json_encode($data);		
