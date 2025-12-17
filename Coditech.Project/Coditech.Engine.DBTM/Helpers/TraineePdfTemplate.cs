namespace Coditech.Engine.DBTM.Helpers
{
    public static class TraineeHtmlTemplate
    {
        public static string GetTemplate()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8' />
    <title>Athlete Profile Report</title>
</head>
<body style='font-family:Arial,Helvetica,sans-serif;margin:20px;color:#222;'>

<div style='max-width:1100px;margin:auto;'>

    <!-- Top Section -->
    <div style='display:grid;grid-template-columns:180px 1fr 220px;gap:20px;align-items:flex-start;'>

        <!-- Photo -->
        <div style='width:180px;height:220px;border:1px solid #ccc;
                    display:flex;align-items:center;justify-content:center;
                    font-size:12px;color:#777;'>
            Athlete Photo
        </div>

        <!-- Basic Info -->
        <div style='border:1px solid #ccc;padding:10px;'>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:160px;'>Name:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#FirstName #LastName</div>
            </div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:160px;'>DOB:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#DOB</div>
            </div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:160px;'>Date of Joining:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#JoiningDate</div>
            </div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:160px;'>Total duration complete:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#TotalDuration</div>
            </div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:160px;'>Trainer Name:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#TrainerName</div>
            </div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:160px;'>Weekly activity hours:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#WeeklyHours</div>
            </div>
        </div>

        <!-- Right Box -->
        <div style='border:1px solid #ccc;padding:10px;'>
            <div style='text-align:center;font-size:40px;margin-bottom:10px;color:#e74c3c;'>⚽</div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:80px;'>Weight:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#Weight</div>
            </div>
            <div style='display:flex;margin-bottom:8px;'>
                <strong style='width:80px;'>Activity:</strong>
                <div style='flex:1;border-bottom:1px solid #999;'>#Activity</div>
            </div>
        </div>

    </div>

    <!-- Score Table -->
    <table style='width:100%;border-collapse:collapse;font-size:14px;margin-top:15px;'>
        <tr>
            <th style='border:1px solid #333;padding:8px;background:#f8c6b8;'>Agility / Speed</th>
            <th style='border:1px solid #333;padding:8px;'>Score</th>
            <th style='border:1px solid #333;padding:8px;background:#f6f08b;'>Power</th>
            <th style='border:1px solid #333;padding:8px;'>Score</th>
            <th style='border:1px solid #333;padding:8px;background:#a8d8a8;'>Endurance</th>
            <th style='border:1px solid #333;padding:8px;'>Score</th>
            <th style='border:1px solid #333;padding:8px;background:#f5a3a3;'>Strength</th>
            <th style='border:1px solid #333;padding:8px;'>Score</th>
            <th style='border:1px solid #333;padding:8px;background:#a8dbf0;'>Flexibility / Mobility</th>
            <th style='border:1px solid #333;padding:8px;'>Score</th>
            <th style='border:1px solid #333;padding:8px;background:#ddd;'>Sport Specific</th>
            <th style='border:1px solid #333;padding:8px;'>Score</th>
        </tr>

        <tr>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>5-0-5</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>#Score505</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>Vertical Jump</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>#VerticalJump</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>MSFT</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>#MSFT</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>1RM</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>#OneRM</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>Sit and Reach</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>#SitReach</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>Bowling speed</td>
            <td style='border:1px solid #333;padding:8px;text-align:center;'>#BowlingSpeed</td>
        </tr>
    </table>

    <!-- Remarks -->
    <div style='margin-top:15px;border:1px solid #333;padding:10px;min-height:60px;'>
        <strong>Remarks:</strong><br />
        #Remarks
    </div>

</div>

</body>
</html>";
        }
    }
}
