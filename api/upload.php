<?php 
	$host = ""; //yourhosthere
	$savedir = ""; //yourdirhere ex images/, i/ etc
	if ($_SERVER["REQUEST_METHOD"] === "POST") {
		define('UPLOAD_DIR', $savedir);
		chdir($_SERVER['DOCUMENT_ROOT']);
		$id = base_convert(microtime(true) * 10000, 10, 36);
		$file = UPLOAD_DIR . $id . '.png';
		$thumb = UPLOAD_DIR . 't/t_' . $id . '.png';
		$success = move_uploaded_file($_FILES["data"]["tmp_name"], $file);
		$success2 = createThumbnail($file, $thumb, 200, 200);
		
		if ($success && $success2) {
			echo "$host/$file";
		}
		else {
			echo "Upload failed";
			http_response_code(500);
		}
	}
	else {
		echo "Only POST method allowed";
		http_response_code(405);
	}
		
	function createThumbnail($filepath, $thumbpath, $thumbnail_width, $thumbnail_height, $background=false) {
		list($original_width, $original_height, $original_type) = getimagesize($filepath);
		if ($original_width > $original_height) {
			$new_width = $thumbnail_width;
			$new_height = intval($original_height * $new_width / $original_width);
		} else {
			$new_height = $thumbnail_height;
			$new_width = intval($original_width * $new_height / $original_height);
		}
		$dest_x = intval(($thumbnail_width - $new_width) / 2);
		$dest_y = intval(($thumbnail_height - $new_height) / 2);
		$old_image = ImageCreateFromPNG($filepath);
		$new_image = imagecreatetruecolor($thumbnail_width, $thumbnail_height);
		imagesavealpha($new_image, TRUE);
		$color = imagecolorallocatealpha($new_image, 0, 0, 0, 127);
		imagefill($new_image, 0, 0, $color);
		imagecopyresampled($new_image, $old_image, $dest_x, $dest_y, 0, 0, $new_width, $new_height, $original_width, $original_height);
		ImagePNG($new_image, $thumbpath);
		return file_exists($thumbpath);
	}
?> 
