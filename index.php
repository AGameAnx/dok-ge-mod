<?php

$patch = basename($_GET['p']);
$layout = basename($_GET['l']);

$url = "";

if ($patch) {
	$url = "https://raw.githubusercontent.com/AGameAnx/dok-repo/master/patches/" . rawurlencode($patch) . ".json";
} elseif ($layout) {
	$url = "https://raw.githubusercontent.com/AGameAnx/dok-repo/master/layouts/" . rawurlencode($layout) . ".dokmap";
}

if (strlen($url) <= 0) {
	http_response_code(403);
	die('Forbidden');
}

$result = file_get_contents($url);
if (empty($result))
{
	http_response_code(404);
	die("Error reading remote file")
}

echo $result;
