<?php
// post params 
// evtype,time,user_name(name string),jobnum,uid(user key),OWNER_CD,name1,name2,name3,name4,name5,name6,
//diem1,diem2,diem3,diem4,diem5,diem6,driver11,driver12,driver13,driver14,driver15,driver16,
//lattitude,longitude
  include "conn.php"; 
  include "helper.php";
  $leader="L"; 
  $event_type=$_POST['evtype'];  
  $event_time =$_POST['time'];
  //$user_name=$_POST['user_name'];
  
   $jobname=$_POST['jobnum']; //Job.number
   $jobkey=$_POST['jobkey'];
  $time=$_POST['time'];
  $uid=$_POST['uid']; 
  $user_name=getUsername($uid);
  //$user_name="test name";
  $owner_cd=$_POST['OWNER_CD'];   
  /////////////Emp id////////////
     $name1=$_POST['name1']; 
     $name2=$_POST['name2']; 
     $name3=$_POST['name3']; 
     $name4=$_POST['name4']; 
     $name5=$_POST['name5']; 
     $name6=$_POST['name6'];

     $emps = array();
     if($name1!=""){
      $emps[] = $name1;
      $name_1=getUsername($name1);
     }
    if($name2!=""){
      $emps[] = $name2;
      $name_2=getUsername($name2);
    }
    if($name3!=""){
      $emps[] = $name3;
      $name_3=getUsername($name3);
    }
    if($name4!=""){
      $emps[] = $name4;
      $name_4=getUsername($name4);
    }
    if($name5!=""){
      $emps[] = $name5;
      $name_5=getUsername($name5);
    }
    if($name6!=""){
      $emps[] = $name6;
      $name_6=getUsername($name6);
    }
  /////////////Emp id////////////
     $diemLeader=$_POST['diemLeader'];
     $diem1=$_POST['diem1']; $diem2=$_POST['diem2'];  $diem3=$_POST['diem3'];
     $diem4=$_POST['diem4']; $diem5=$_POST['diem5']; $diem6=$_POST['diem6'];

  //////////Empname (first name last name)///////////////

    //  $name_1=$_POST['name_1']; 
    //  $name_2=$_POST['name_2']; 
    //  $name_3=$_POST['name_3']; 
    //  $name_4=$_POST['name_4']; 
    //  $name_5=$_POST['name_5']; 
    //  $name_6=$_POST['name_6'];
  ///////////////////////////////

    //  $_SESSION['name11']=$_POST['name1']; 
    //  $_SESSION['name21']=$_POST['name2']; 
    //  $_SESSION['name31']=$_POST['name3']; 
    //  $_SESSION['name41']=$_POST['name4']; 
    //  $_SESSION['name51']=$_POST['name5']; 
    //  $_SESSION['name61']=$_POST['name6'];
   /////////////Drivers////////////////////
     $driver11=$_POST['driver11'];
     $driver22=$_POST['driver22'];
     $driver33=$_POST['driver33'];
     $driver44=$_POST['driver44'];
     $driver55=$_POST['driver55'];
     $driver66=$_POST['driver66'];  
	 
	 
	 
	 
	 $job_phase_number = $_POST['job_phase'];

     $emps[] = $uid;
     $eps="";
     $emplen=count($emps);
     $resar=[];
     $_SESSION['crew_session'] = array();
    
     for($i=0;$i<$emplen;$i++){
      $emp=$emps[$i];
        $sqlemp="select * from timesheet where employee='$emp' and event_type='15' and  DATE(event_time)=DATE(now())";
        $empres=mysqli_query($con,$sqlemp);
        if(mysqli_num_rows($empres)==0){
          if($eps=="")
         $eps.= $emp;  else  $eps.= ",".$emp;
          $resar[]=$emp;
         // array_push($_SESSION['crew_session'],$emp);
        }
        $getidsql="select * from end_user where `key`='$emp' and record_state='L'";
        $getidquery=mysqli_query($con,$getidsql);
        $getidres=mysqli_fetch_assoc($getidquery);
        $emp_id=$getidres['id'];
  //job_working,job_assigned_on,crew_leader,crew_leader_assigned_on
        $updatesql="update end_user set job_working='$jobname',job_assigned_on='$time',crew_leader='$uid',crew_leader_assigned_on='$time' where id='$emp_id'";
        mysqli_query($con,$updatesql);         
     }
    // $_SESSION['crewemps']=$eps;

//if(isset($_SESSION['uid'])) {
  $uid=$_POST['uid'];

  //////////delete old event of this day////////////// 
    $selsql="select * from crew_default where crew_leader='$uid'";  
  $selres=mysqli_query($con,$selsql);
      while($selrow=mysqli_fetch_array($selres)){
      $id=$selrow['id'];
      $delquery="delete from crew_default where id='$id'";   
      mysqli_query($con,$delquery);
    }
  ///////////////Save Job event////////////////////////

     $latitude=$_POST['lattitude'];
     $longitude=$_POST['longitude'];
     $sql="insert into Job_crew(job, job_phase_number, date,employee,leader,name) values('$jobname','$job_phase_number' ,'$time','$uid','L','$user_name')";
    mysqli_query($con,$sql);
    echo $sql;
    //
    $sqleventu="insert into job_events(job_key, job, job_phase_number, employee, leader, event_time, event_type,latitude,longitude,per_diem) 
	values('$jobkey', '$jobname', '$job_phase_number', $uid', 'L', '$event_time', '$event_type','$latitude','$longitude','$diemLeader')";    
    mysqli_query($con,$sqleventu);
    /////////////////////******************************

  if(isset($_POST['name1']) && $_POST['name1']!="") {
    if($name1==$uid) $leader='L'; else $leader='C';
    $sql="insert into Job_crew(job,job_phase_number, date,employee,leader,name) values('$jobname','$job_phase_number','$time','$name1','$leader','$name_1')";
    mysqli_query($con,$sql);
    ///for crew_default 
    $check="select * from crew_default where crew_leader='$uid' and team_member='$name1'";
    $chkres=mysqli_query($con,$check);
     if(mysqli_num_rows($chkres)>0){     
      $crew_update_query="update crew_default set created_on='$time' where crew_leader='$uid' and team_member='$name1'";
     mysqli_query($con,$crew_update_query);
    } else {
      $crew_default_query="insert into crew_default (crew_leader, team_member, created_on) values('$uid','$name1','$time')";
     mysqli_query($con,$crew_default_query);
    }

  $getusersql1="select labor_classification from end_user where `key`='$name1' and record_state='L'";
  $userres1=mysqli_query($getusersql1,$con);
  $getres1=mysqli_fetch_assoc($userres1);
  ///////////////////
  $classification=$getres1['labor_classification'];
  $dm=NULL;
  $sqlevent="insert into job_events(job_key,job, job_phase_number, employee, leader, event_time, event_type,latitude,longitude,per_diem,driver) 
  values('$jobkey','$jobname', '$job_phase_number', '$name1', 'C', '$event_time', '$event_type','$latitude','$longitude','$diem1','$driver11')";    
    $res=mysqli_query($con,$sqlevent);

  }
  if(isset($_POST['name2']) && $_POST['name2']!="") {
    if($name2==$uid) $leader='L'; else $leader='C';
    $sqlevent="insert into Job_crew(job,job_phase_number,date,employee,leader,name) values('$jobname','$job_phase_number','$time','$name2','$leader','$name_2')";
    mysqli_query($con,$sqlevent);
    ///for crew_default
     $check="select * from crew_default where crew_leader='$uid' and team_member='$name2'";
    $chkres=mysqli_query($con,$check);
     if(mysqli_num_rows($chkres)>0){  
      $crew_update_query="update crew_default set created_on='$time' where crew_leader='$uid' and team_member='$name2'";
     mysqli_query($con,$crew_update_query);
    } else {
      $crew_default_query="insert into crew_default (crew_leader, team_member, created_on) values('$uid','$name2','$time')";
     mysqli_query($con,$crew_default_query);
    }
  ////Insert into job event///////
  $getusersql2="select labor_classification from end_user where `key`='$name2' and record_state='L'";
  $userres2=mysqli_query($getusersql2,$con);
  $getres2=mysqli_fetch_assoc($userres2);
  ///////////////////
  $sql="insert into job_events(job_key, job,job_phase_number, employee, leader, event_time, event_type,latitude,longitude,per_diem,driver) 
  values('$jobkey','$jobname','$job_phase_number', '$name2', 'C', '$event_time', '$event_type','$latitude','$longitude','$diem2','$driver22')";    
    $res=mysqli_query($con,$sql);
  }
  if(isset($_POST['name3']) && $_POST['name3']!="") {
    if($name3==$uid) $leader='L'; else $leader='C';
    $sql="insert into Job_crew(job,job_phase_number,date,employee,leader,name) values('$jobname','$job_phase_number','$time','$name3','$leader','$name_3')";
    mysqli_query($con,$sql);
    ///for crew_default
    $check="select * from crew_default where crew_leader='$uid' and team_member='$name3'";
    $chkres=mysqli_query($con,$check);
     if(mysqli_num_rows($chkres)>0){ 
      $crew_update_query="update crew_default set created_on='$time' where crew_leader='$uid' and team_member='$name3'";
     mysqli_query($con,$crew_update_query);   
    } else {
      $crew_default_query="insert into crew_default (crew_leader, team_member, created_on) values('$uid','$name3','$time')";
     mysqli_query($con,$crew_default_query);
    }
    ////Insert into job event///////
    $getusersql3="select labor_classification from end_user where `key`='$name3' and record_state='L'";
  $userres3=mysqli_query($getusersql3,$con);
  $getres3=mysqli_fetch_assoc($userres3);
  ///////////////////
  $dm="NULL";
  $sqlevent="insert into job_events(job_key, job, job_phase_number, employee, leader, event_time, event_type,latitude,longitude,per_diem,driver) 
  values('$jobkey','$jobname', '$job_phase_number','$name3', 'C', '$event_time', '$event_type','$latitude','$longitude','$diem3','$driver33')";    
    $res=mysqli_query($con,$sqlevent);
  }
  if(isset($_POST['name4']) && $_POST['name4']!="") {
     if($name4==$uid) $leader='L'; else $leader='C';
    $sql="insert into Job_crew(job,date,job_phase_number, employee,leader,name) values('$jobname','$job_phase_number',$time','$name4','$leader','$name_4')";
    mysqli_query($con,$sql);
    ///for crew_default
     $check="select * from crew_default where crew_leader='$uid' and team_member='$name4'";
    $chkres=mysqli_query($con,$check);
     if(mysqli_num_rows($chkres)>0){  
      $crew_update_query="update crew_default set created_on='$time' where crew_leader='$uid' and team_member='$name4'";
     mysqli_query($con,$crew_update_query);   
    } else {
      $crew_default_query="insert into crew_default (crew_leader, team_member, created_on) values('$uid','$name4','$time')";
     mysqli_query($con,$crew_default_query);
    }
    ////Insert into job event///////
   $getusersql4="select labor_classification from end_user where `key`='$name4' and record_state='L'";
  $userres4=mysqli_query($getusersql4,$con);
  $getres4=mysqli_fetch_assoc($userres4);
  ///////////////////
  $dm=NULL;
  $sqlevent="insert into job_events(job_key, job, job_phase_number, employee, leader, event_time, event_type,latitude,longitude,per_diem,driver) 
  values('$jobkey','$jobname', '$job_phase_number',$name4', 'C', '$event_time', '$event_type','$latitude','$longitude','$diem4','$driver44')";    
    $res=mysqli_query($con,$sqlevent);
  }
  if(isset($_POST['name5']) && $_POST['name5']!="") {
    if($name5==$uid) $leader='L'; else $leader='C';
    $sql="insert into Job_crew(job,job_phase_number, date,employee,leader,name) values('$jobname','$job_phase_number','$time','$name5','$leader','$name_5')";
    mysqli_query($con,$sql);
    ///for crew_default
     $check="select * from crew_default where crew_leader='$uid' and team_member='$name5'";
    $chkres=mysqli_query($con,$check);
     if(mysqli_num_rows($chkres)>0){    
      $crew_update_query="update crew_default set created_on='$time' where crew_leader='$uid' and team_member='$name5'";
     mysqli_query($con,$crew_update_query); 
    } else {
      $crew_default_query="insert into crew_default (crew_leader, team_member, created_on) values('$uid','$name5','$time')";
     mysqli_query($con,$crew_default_query);
    }
    ////Insert into job event///////
  $getusersql5="select labor_classification from end_user where `key`='$name5' and record_state='L'";
  $userres5=mysqli_query($getusersql5,$con);
  $getres5=mysqli_fetch_assoc($userres5);
  ///////////////////
  $dm=NULL;
  $sqlevent="insert into job_events(job_key, job, employee, leader, event_time, event_type,latitude,longitude,per_diem,driver) 
  values('$jobkey','$jobname', '$name5', 'C', '$event_time', '$event_type','$latitude','$longitude',NULL,'$diem5','$driver55')";    
    $res=mysqli_query($con,$sqlevent);
  }
  if(isset($_POST['name6']) && $_POST['name6']!="") {
    if($name6==$uid) $leader='L'; else $leader='C';
    $sql="insert into Job_crew(job,job_phase_number, date,employee,leader,name) values('$jobname','$job_phase_number','$time','$name6','$leader','$name_6')";
    mysqli_query($con,$sql);
    ///for crew_default
     $check="select * from crew_default where crew_leader='$uid' and team_member='$name6'";
    $chkres=mysqli_query($con,$check);
     if(mysqli_num_rows($chkres)>0){     
       $crew_update_query="update crew_default set created_on='$time' where crew_leader='$uid' and team_member='$name6'";
     mysqli_query($con,$crew_update_query);
    } else {
      $crew_default_query="insert into crew_default (crew_leader, team_member, created_on) values('$uid','$name6','$time')";
     mysqli_query($con,$crew_default_query);
    }
     ////Insert into job event///////
  $getusersql6="select labor_classification from end_user where `key`='$name6' and record_state='L'";
  $userres6=mysqli_query($getusersql6,$con);
  $getres6=mysqli_fetch_assoc($userres6);
  ///////////////////
  $dm=NULL;
  $sqlevent="insert into job_events(job_key, job, job_phase_number, employee, leader, event_time, event_type,latitude,longitude,per_diem,driver) 
  values('$jobkey', '$jobname', '$job_phase_number', '$name6', 'C', '$event_time', '$event_type','$latitude','$longitude','$diem6','$driver66')";    
    $res=mysqli_query($con,$sqlevent);
  }
  if($res){
     if($eps=="")
    echo "1";
     else 
    echo "0"; 
  } else {
    echo "0";
  }
     
?>
