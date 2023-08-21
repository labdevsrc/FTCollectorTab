 <?php 
 // session_start();
  include 'conn.php';
  ////////////variables//////////////////  
 
  $job=$_POST['jobnum']; 
  $jobkey=$_POST['jobkey']; 
  $employee=$_POST['uid'];
  $job_phase_number = $_POST['job_phase'];
  $leader="L";
  $mileshours=$_POST['miles_hours'];  
  $perDiem=$_POST['perdiem'];
  //$event_time="now()";
  //$event_type="2"; 
  // Caller from : job.php line 171
  // data:{"jobnumber":jobnumber,"time":time,"evtype":"2","longitude2":longitude2,"lattitude2":lattitude2},
  $event_type=$_POST['evtype']; //#1 , evtype ?
  
  $hr ="";
  $min ="";
  $event_time =$_POST['time'];
  if(isset($_POST['hr']) && $_POST['hr']!="")
  $hr =$_POST['hr'];
  if(isset($_POST['min']) && $_POST['min']!="")
  $min =$_POST['min'];
  
  // this sql never executed ???
  // because $event_type="2"
  if(isset($_POST['hr']) && $_POST['hr']!="" && isset($_POST['min']) && $_POST['min']!="" && ($event_type=="13" || $event_type=="14") ){
  $sql="insert into timesheet (employee,event_time,event_type,user_hour,user_minute,employee_approved,per_diem_approved) values('$employee','$event_time','$event_type','$hr','$min','$event_time','0')";
  mysqli_query($con,$sql);
  
}
  
  

  // if(isset($_POST['jobnumber'])){
  //   $job=$_POST['jobnumber'];
  // } else if(isset($_SESSION['jobnum'])){
  //   $job=$_SESSION['jobnum'];
  // }
$latitude="";
$longitude="";
  //////////////////Fetch GPS////////////////////
  $getgps="select * from live_GPS_position where when_recorded=(select MAX(when_recorded) from live_GPS_position where user_id='$employee')";
  $resgps=mysqli_query($con,$getgps);  
  $gpsrow=mysqli_fetch_array($resgps);
  

  $latitude=$gpsrow['lattitude'];
  $longitude=$gpsrow['longitude'];
  ////
  //if(isset($_SESSION['mainlat']) && $_SESSION['mainlat']!=0) {
  if($_POST['mainlat']!=0 || $_POST['mainlat']!="0"){
    $latitude=$_SESSION['mainlat'];  
    $longitude=$_SESSION['mainlong'];
 }
//}
  //$_SESSION['mainlong']

        ////////////Manual Gps////////////
        if($_POST['gps_sts']=="0"){
          $latitude=$_POST['manual_latti'];
          $longitude=$_POST['manual_longit'];
        ////////////////////////////////////////
        } else {
          /////////Get live GPS//////////
        // $userid=$_SESSION['uid'];
        // $sqlgps="select * from gps_tracking where user_id='$userid' and when_recorded=(select max(when_recorded) from gps_tracking where user_id='$userid')";
        // $resgps=mysqli_query($con,$sqlgps);
        // $rowgps=mysqli_fetch_assoc($resgps);     
        // $datetime = explode(" ",$rowgps['when_recorded']);
        // $date = $datetime[0];
        // $time = $datetime[1];
        // $latitude=$rowgps['lattitude'];
        // $longitude=$rowgps['longitude'];
           $longitude=$_POST['longitude2'];
           $latitude=$_POST['lattitude2'];

///////////////////End///////////////////////////
        }

  ////Insert///////
  if($event_type=="14"){
    $_POST['lunchinsts']='1';
  }

  $sql="insert into job_events(job,job_key, employee, leader, 
  job_phase_number, miles_hours,
  event_time, event_type,latitude,longitude,per_diem) values('$job', '$jobkey','$employee', '$leader',
  '$job_phase_number',  '$mileshours',
  '$event_time', '$event_type','$latitude','$longitude','$perDiem')"; 
  
  //if(isset($_SESSION['jobnum']) && isset($_SESSION['uid']) ){
  if(isset($_POST['uid']) ){
    $res=mysqli_query($con,$sql);
    echo "Job :".$job.",employe : ".$employee.",Status 1, Lattitude".$latitude.", long : ".$longitude;
  } else {
    echo "Job :".$job.",employe : ".$employee.",Status 0, Lattitude".$latitude.", long : ".$longitude;
  }
  /////////////////////////////////////////

  ?>