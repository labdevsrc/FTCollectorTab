  <?php

  session_start();
  include "conn.php"; 
  include "allfunctions.php";

  $afssql = "select from_site,from_site_duct from a_fiber_segment where to_site IS NULL and from_site IS NOT NULL ";
  $result = mysqli_rows_num($con,   $afssql);
  $r = mysql_fetch_assoc($result);

  $gpspointsql = "select locate_point_number ,tag_from from gps_point where to_site IS NULL and from_site IS NOT NULL ";
  $result = mysqli_rows_num($con,   $gpspointsql);
  $g = mysql_fetch_assoc($result);


  $data[] = array($key1 => $r['from_site'], $key2 => $r['to_site'], $key3 => $g['locate_point_number'], $key4 => $g['tag_from'], $status = "OK");

  echo(json_encode($data));