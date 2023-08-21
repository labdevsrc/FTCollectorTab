<?php
  /// This script invoke by GetSiteFromAWSMySQLTable(), Constants.GetSiteTableUrl


  include "conn.php";
  
  $sql="select Site.key SiteKey, Site.job_key JobKey, Site.owner_key OwnerKey, OWNER_CD , tag_number TagNumber, 
site_id SiteId, SiteName, LONGITUDE, 
LATITUDE, Site.job  JobNumber, cs.major_type SiteTypeDesc ,cs.`key` SiteTypeKey , direction_of_travel as DirOfTravel , created_by as CreatedBy 
from Site
inner join (select `key`,major_type from code_site_type where code_site_type.record_state='L') as cs on cs.key = Site.type
     where Site.record_state = 'L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }



  echo json_encode($data);

?>
