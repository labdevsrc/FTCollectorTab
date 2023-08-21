<?php
	require 'aws/aws-autoloader.php';
	include "conn.php";
	session_start();
	 $time=$_POST['time'];  
   $namestr=$_POST['namestr'];  
	 $timesig=$_POST['timesig'];
	 $img=$_POST['imagedt'];
	 $ownerCD=$_SESSION['OWNER_CD'];
	 $tag=$_SESSION['tag'];
   $uid=$_SESSION['uid'];
   $okey=$_SESSION['owner_key'];
	  //$time='now()';
	  $type=$_SESSION['type'];

	  if($type == "Building"){
	  	$site_type="1";
	  } else if($type == "Cabinet"){
	  	$site_type="2";
	  } else if($type == "Pole"){
	  	$site_type="3";
	  } else if($type == "Pull Box"){
	  	$site_type="4";
	  }
//aws upload
	  $imgData = str_replace(' ','+',$img);
           
            $imgData =  substr($imgData,strpos($imgData,",")+1);
            $imgData = base64_decode($imgData);

    $s3 = new Aws\S3\S3Client([
      'region'  => 'us-east-2',
      'version' => 'latest',
      'credentials' => [
        'key'    => "AKIAJTM6EJOVYMZEVPPQ",
        'secret' => "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",
      ]
    ]);   

    $result = $s3->putObject([
      'Bucket' => 'fibertrakmedia',
      //'Key'    => $file_name,
      //'Key'    => 'signature_'.$timesig.'_'.$uid.'.png',
      'Key'    => $namestr,
      //'SourceFile' => $temp_file_location  
      'Body'            => $imgData,
        'ContentType'     => 'image/png',
        'ACL'             => 'public-read' 
         
    ]);
    ////end qws upload



	  //$imgData = str_replace(' ','+',$img);
	 //$imgData =  substr($imgData,strpos($imgData,",")+1);

  //    $imgData = base64_decode($img);
	 // $sql="insert into signature_test (signature) values('$img')";
	 // mysqli_query($con,$sql);
	 // echo "saved";


	 //$fname=$owner."_".$tag."_".$user."_".$page."_".$longitude."_".$lattitude."_".date('Y-m-d-H-i-s').".png";
	  $imgData = str_replace(' ','+',$img);           
      $imgData =  substr($imgData,strpos($imgData,",")+1);
      $imgData = base64_decode($imgData);
	  $fname="signature".$time.".png";
            
            $filePath ='images/'.$fname;
            $_SESSION['img_path']= $filePath;
           
            $file = fopen($filePath, 'w');
            fwrite($file, $imgData);
            fclose($file);

            //Save into DB

              $sql1="update Site set signed_on='$time',completed='1' where tag_number='$tag' and OWNER_CD='$ownerCD' and record_state='L'";

            $sql="insert into pictures (owner,when_taken,picture_file_name,copied,created_on,created_by) 
              values('$okey','$time','$namestr','$time','$time','$uid')";
              if(isset($_SESSION['OWNER_CD'])){
              $res=mysqli_query($con,$sql);
             	mysqli_query($con,$sql1);
             }
             $jobeventrowid=$_SESSION['equiprowid'];
             $updatesql="update job_events set signature='$fname' where id='$jobeventrowid'";
             mysqli_query($con,$updatesql);             
            //


             $msg= array("sts" => "Signature has been saved","purl" => $filePath,"fname" => $fname);
             	//$_SESSION['checkin']="1"; 24-7-19
               $_SESSION['checkin']="0";
              echo json_encode($msg);




              	//where tag_number='$tag_number1' and type='1'";



















	 // $sql1="select * from signature_test where id='2'";
	 // $res=mysqli_query($con,$sql1);
	 // $row=mysqli_fetch_row($res);
	 // $imgData = str_replace(' ','+',$row['1']);           
  //   $imgData =  substr($imgData,strpos($imgData,",")+1);
  //   $imgData = base64_decode($imgData);
  //   //header("Content-Type:" . "image/jpeg");
  //   echo "<img src='".$row['1']."'/>";
  //   //echo $row['1'];



	//  $sql1="select * from signature_test where id='2'";
	//  $res=mysqli_query($con,$sql1);
	//  $row=mysqli_fetch_row($res);
	// header("Content-Type:" . "image/png");
	//  $imgData = str_replace(' ','+',$row['signature']);
           
 //            $imgData =  substr($imgData,strpos($imgData,",")+1);
 //            $imgData = base64_decode($imgData);
	//  echo base64_encode($row['signature']);
	 //echo '<img src="data:image/jpeg;base64,'. base64_encode($row['1']) .'" />';
	// echo $row['1'];





// $sql = "SELECT * FROM images where id= '1'";++

// $result = mysqli_query($con,$sql);

// while($row=mysqli_fetch_assoc($result)) {



// header('content-type: image/jpeg');

// echo $a=$row['img'];

// echo base64_decode($a); 



// }
//echo "img";

//$conn->close();


	
 ?>
