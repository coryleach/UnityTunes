
echo "Running"

$SPOTIFY_PLAYPAUSE = 917504
$SPOTIFY_MUTE = 524288
$SPOTIFY_STOP = 851968
$SPOTIFY_PREVIOUS = 786432
$SPOTIFY_NEXT = 720896

$SPOTIFY_PROCESS_NAME = "Spotify"
$SPOTIFY_NOTHING_PLAYING_WINDOW_TITLE = "Spotify"

function IsSpotifyRunning()
{
    (Get-Process | Where-Object { $_.Name -eq $SPOTIFY_PROCESS_NAME }).Count -gt 0
}

function GetSpotifyWindowTitle()
{
    $(Get-Process |where {$_.mainWindowTitle -and $_.name -eq $SPOTIFY_PROCESS_NAME} | select mainwindowtitle).MainWindowTitle
}

function PlayPause()
{
    SendSpotifyCommand($SPOTIFY_PLAYPAUSE);
}

function IsPlaying()
{
    $(GetSpotifyWindowTitle) -and $(GetSpotifyWindowTitle) -ne $SPOTIFY_NOTHING_PLAYING_WINDOW_TITLE
}

function SendSpotifyCommand($command)
{

$spotifyProcess = Get-Process | Where-Object {$_.MainWindowTitle -and $_.Name -eq "Spotify"} | Select-Object Id,Name,MainWindowHandle,MainWindowTitle
#Store the C# signature of the SendMessage function. 
$signature = @"
[DllImport("user32.dll")]
public static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
"@
#Add the SendMessage function as a static method of a class
$SendMessage = Add-Type -MemberDefinition $signature -Name "Win32SendMessage" -Namespace Win32Functions -PassThru
#Invoke the SendMessage Function
$result = $SendMessage::SendMessage($spotifyProcess.MainWindowHandle, 0x0319, 0, $command)

}

if (IsSpotifyRunning())
{
    echo "PlayPause"
    PlayPause();
}