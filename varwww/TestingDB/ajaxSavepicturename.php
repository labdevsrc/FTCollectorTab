 <?php 
 // session_start();
  include 'conn.php';
  include "allfunctions.php";
  $time=$_POST['time']; 
  $fname=$_POST['fname']; 
  $lattitude=$_POST['lattitude']; 
  $longitude=$_POST['longitude'];
   $pg=$_POST['page'];
  $tag=$_POST['tag'];
  if($tag==""){
    $sitekey="";
  }else{
  $sitekey=getHostkey($tag);
}
  $ownerkey=$_POST['owner_key'];
  //$owner=$_SESSION['oid'];
   
   
$user=$_POST['uid'];
    //$plansheet=$_SESSION['plansheet'];
    //$psitem=$_SESSION['psitem'];
    //////////////////////////////
      $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'pictures'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT'];
///////////////////End/////////////////////////// 
            $sql="insert into pictures (`key`,owner_key,site_key,when_taken,picture_file_name,end_user_key,page,longitude,lattitude,copied,created_on,created_by) 
              values('$autoid','$ownerkey','$sitekey','$time','$fname','$user','$pg','$longitude','$lattitude','$time','$time','$user')";
              $res=mysqli_query($con,$sql);
              if($res){
              $msg= array("sts" => "Photo has been saved");
              echo json_encode($msg);          


  } else {
       $msg= array("sts" => "Failed to saved photo");
       echo json_encode($msg);

  }

  ?>