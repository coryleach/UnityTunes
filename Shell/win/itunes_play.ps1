# Check if process is running
$isRunning = Get-Process | Where-Object ProcessName -Like "*itunes*"

if ( -not $isRunning )
{
    #iTunes is not running
    return
}

#Get iTunes Com Object
$itunes = New-Object -ComObject iTunes.Application

function Check-Running {
    process {
        $itunesRunning = Get-Process | Where-Object ProcessName -Like "*itunes*"

        if ($itunesRunning)
        {

        }

        # Initializing itunes
        $itunes = New-Object -ComObject iTunes.Application

        # list methods and properties
        $itunes | Get-Member
    }
}


if ( -not ($itunes.CurrentTrack) )
{
    #No track is selected
    
}
elseif ( ($itunes.PlayerState -eq 1) )
{
    #player state is playing
    
}
else
{
    #player state is not playing
    echo "Play"
    $itunes.Play()
}
