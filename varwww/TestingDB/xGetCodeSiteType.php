<?php 
  include "conn.php";
  $sql="select code_site_type.key as CodeKey, site_type as SiteType, major_type as MajorType, minor_type as MinorType, ITSFM from code_site_type where record_state='L'"; 
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }

  echo json_encode($data);    
?>