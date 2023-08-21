	<?php
	session_start();
	include "conn.php";	
  include "allfunctions.php";
  //$ownerCD=$_SESSION['oid'];
  //$time=$_POST['time']; 
  // {"idaddr":idaddr,"subnet":subnet,"protocal":protocal,"vidioproto":vidioproto,"getway":getway,"mulip":mulip,"vlan":vlan},

 $rowid=$_POST['rowid'];
 $idaddr=$_POST['ipaddr'];
 $subnet=$_POST['subnet'];
 $protocal=$_POST['protocol'];
 $vidioproto=$_POST['vidioproto'];
 $getway=$_POST['gateway'];
 $mulip=$_POST['multicastip'];
 $vlan=$_POST['vlan'];
 //$time=$_POST['time'];
 $updsql="update chassis set IP_address='$idaddr',subnet_mask='$subnet',protocol='$protocal',video_protocol='$vidioproto',IP_gateway='$getway',multicast_IP='$mulip',vlan='$vlan' where `id`='$rowid'";
 mysqli_query($con,$updsql);
 

  ?>