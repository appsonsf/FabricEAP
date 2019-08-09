#
# AddPlacementConstraintsIntoApplicationManifest.ps1
#

Param
(
	[string]$ApplicationManifestFilePath = [string]::Empty,

	[string]$ServiceName = [string]::Empty,

	[string]$PlacementConstraints = [string]::Empty,

	[string]$ServiceType = "StatelessService"
)

$ApplicationManifestFilePath = Resolve-Path -Path $ApplicationManifestFilePath
if (!(Test-Path $ApplicationManifestFilePath -PathType Leaf)) {
	throw "Could not locate the ApplicationMenifest file to update at the path '$ApplicationManifestFilePath'."
}

function Set-XmlNodesElementTextValue([xml]$xml, $node, $elementName, $textValue)
{
	if ($null -eq $node.($elementName))
	{
		$element = $xml.CreateElement($elementName, $xml.DocumentElement.NamespaceURI)		
		$textNode = $xml.CreateTextNode($textValue)
		$element.AppendChild($textNode) > $null
		$node.AppendChild($element) > $null
	}
	else
	{
		$node.($elementName) = $textValue
	}
}


[xml]$xml = Get-Content -Path $ApplicationManifestFilePath

$services = $xml.GetElementsByTagName('Service') 

foreach ($service in $services) {
	if ($service.Name -eq $ServiceName) {

		$serviceInstance = $service.($ServiceType)

		Set-XmlNodesElementTextValue -xml $xml -node $serviceInstance -elementName 'PlacementConstraints' -textValue "$PlacementConstraints"

		break
	}
}

$xml.Save($ApplicationManifestFilePath)

