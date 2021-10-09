@findstr /B /V @ %~dpnx0 > %~dpn0.ps1 && powershell -ExecutionPolicy Bypass %~dpn0.ps1 %*
@exit /B %ERRORLEVEL%
param
(
    [Parameter(Mandatory)]
    [string]$r = "",
    [Parameter(Mandatory)]
    [string]$e = ""
)

$aws_identity_data = aws sts get-caller-identity | ConvertFrom-Json 
$aws_account_id = $aws_identity_data.Account
cdk bootstrap aws://$aws_account_id/$r -c envName=$e
cdk deploy -c envName=$e --require-approval=never
exit $lastExitCode