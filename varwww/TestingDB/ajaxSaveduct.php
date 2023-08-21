	<?php
	session_start();
	include "conn.php";	
  include "allfunctions.php"; 	
  $time=$_POST['time']; 
	$htag=$_POST['host_tag_number'];
  $direction=$_POST['direction'];
  $dcount=$_POST['direction_count'];
  $dsize=$_POST['duct_size'];
  $dcolor=$_POST['duct_color'];
  $dtype=$_POST['duct_type'];
  /*if($_SESSION['sitename']=="building") {
    $htype="1";
  }else if($_SESSION['sitename']=="cabinet") {
      $htype="2";
  }else if($_SESSION['sitename']=="structure") {
      $htype="3";
  }else if($_SESSION['sitename']=="pull box") {
      $htype="4";
  }*/
  
  $htype=$_POST['site_type_key'];

  $dusage=$_POST['duct_usage'];
  $dgrouptype=$_POST['duct_grouptype'];
  $dgroupid=$_POST['duct_groupid'];
  $dinuse=$_POST['duct_inuse'];
  $dtrace=$_POST['duct_trace'];
  $dpull=$_POST['has_pull_tape'];
  $uid=$_POST['uid'];
  $ownerCD=$_POST['OWNER_CD'];
  $openpercent=$_POST['openpercent'];
  $seal=$_POST['seal'];
  $install=$_POST['install'];
  
  
  //13   
     ////*******Getting owner key******
  $ownerkeysql="select * from `owner` where OWNER_CD='$ownerCD' and record_state='L'";
  $resoquery=mysqli_query($con,$ownerkeysql);
  $resokey=mysqli_fetch_assoc($resoquery);
  $ownerkey=$resokey['key'];
  ////*******Getting job key********
  $jobkeysql="select * from Job where OWNER_CD='$ownerCD' and record_state='L'";
  $resjobquery=mysqli_query($con,$jobkeysql);
  $resjobkey=mysqli_fetch_assoc($resjobquery);
  $jobkey=$resjobkey['key'];
  //****
   $id=0;
   // this sql3 will update already existed / replaced row to record_state=H
    $sql3="update CONDUITS_GROUP 
   set updated_on='$time',record_state='H' where OWNER_CD='$ownerCD' and host_tag_number='$htag' and host_type='$htype' and record_state='L' "; 

   // this sql2 will check any new row existed with record_state = L
     $sql2="select * from CONDUITS_GROUP where host_tag_number='$htag' and host_type='$htype' and OWNER_CD='$ownerCD' and record_state='L'";
     $insert_accepoint="insert into CONDUITS_ACCESS_POINT (tag_number,created_on,created_by) values('$htag','$time','$uid')";
    $sql1="select max(id) as big from CONDUITS_GROUP";
    $res=mysqli_query($con,$sql1);
    $row=mysqli_fetch_array($res);
    if($row['big'] >=0){
       $id=$row['big']+1;
    }
    $msg= array("d" => $row['big'],"cnumber" => "","actnumber"=>"","postion"=>"","sts"=>"0");


if(isset($htag)){
  $hostkey=getHostkey($htag);
  $resstmt=mysqli_query($con,$sql2);
      if(mysqli_num_rows($resstmt)>0){
        //mysqli_query($con,$sql3);
        mysqli_query($con,$insert_accepoint);
        $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'CONDUITS_GROUP'";
        $autoquery=mysqli_query($con,$autoidquery);
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT'];
        //$res=mysqli_fetch_assoc($resstmt);
        $ky=$autoid;
        $sql="insert into CONDUITS_GROUP(id,host_tag_number,direction,direction_count,
    DUCT_SIZE_CD,color,USAGE_CD,CONDUIT_GROUP_TYPE,CONDUIT_GROUP_ID,
    in_use,in_use_percent,has_trace_wire,has_pull_tape,created_by,created_on,OWNER_CD,host_type,DUCT_TYPE_CD,has_seal,`key`,OWNER_KEY,host_key,pull_tape_type_key,DUCT_INSTALLATION_CD,created_from) values('$id','$htag','$direction',
    '$dcount','$dsize','$dcolor','$dusage','$dgrouptype','$dgroupid',
    '$dinuse','$openpercent','$dtrace','$dpull','$uid','$time','$ownerCD','$htype','$dtype','$seal','$ky','$ownerkey','$hostkey','$dpull','$install','$createdfrm')";

     $sql4="select * from CONDUITS_GROUP where host_tag_number='$htag' and host_type='$htype' and OWNER_CD='$ownerCD' and record_state='L' and direction_count = '$dcount'";
        mysqli_query($con,$sql);
        $res3  = mysqli_query($con,$sql4); // verify
        $_SESSION['pb']="0";
        if(mysqli_num_rows($res3) > 0 ){
          echo json_encode($msg);
        }
        else{
          $msg= array("d" => "Insert Fail","cnumber" => "","actnumber"=>"","postion"=>"","sts"=>"4");
          echo json_encode($msg);
        }
      }else{
        //get new key
         $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'CONDUITS_GROUP'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

        $sql="insert into CONDUITS_GROUP(id,host_tag_number,direction,direction_count,
    DUCT_SIZE_CD,color,USAGE_CD,CONDUIT_GROUP_TYPE,CONDUIT_GROUP_ID,
    in_use,in_use_percent,has_trace_wire,has_pull_tape,created_by,created_on,OWNER_CD,host_type,DUCT_TYPE_CD,has_seal,`key`,OWNER_KEY,host_key,pull_tape_type_key,DUCT_INSTALLATION_CD,created_from) values('$id','$htag','$direction',
    '$dcount','$dsize','$dcolor','$dusage','$dgrouptype','$dgroupid',
    '$dinuse','$openpercent','$dtrace','$dpull','$uid','$time','$ownerCD','$htype','$dtype','$seal','$autoid','$ownerkey','$hostkey','$dpull','$install','$createdfrm')";
    mysqli_query($con,$sql);
    $res2  = mysqli_query($con,$sql2);
    mysqli_query($con,$insert_accepoint);
    $_SESSION['pb']="0";
        if(mysqli_num_rows($res2) > 0 ){
          echo json_encode($msg);
        }
        else{
          $msg= array("d" => "CONDUIT_GROUP Insert Fail","cnumber" => "","actnumber"=>"","postion"=>"","sts"=>"4");
          echo json_encode($msg);
        }
      }
  }
  else{
      $msg= array("d" => "No Host Tag Number","cnumber" => "","actnumber"=>"","postion"=>"","sts"=>"5");
      echo json_encode($msg);
  }
	
?>
