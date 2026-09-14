using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Control;
using Windows.Storage.Streams;
using System.Text.Json;

class Puroguramu
{
    static async Task Main(string[] hikisuu)
    {
        if (hikisuu.Length == 0 || hikisuu[0] == "-?" || hikisuu[0] == "--herupu")
        {
            Console.WriteLine("使用法: Kuikkusongu.exe [オプション]");
            Console.WriteLine("オプション:");
            Console.WriteLine("  -namae         : 現在の曲名を表示");
            Console.WriteLine("  -aatisuto      : 現在のアーティスト名を表示");
            Console.WriteLine("  -setsumei      : 「タイトル - アーティスト」を表示");
            Console.WriteLine("  -aikon         : アルバムアートをbase64データ(png)で表示");
            Console.WriteLine("  -jikan         : 開始時間、終了時間、経過時間をjsonで表示");
            Console.WriteLine("  -joutai        : 状態を表示: Closed, Opened, Changing, Stopped, Playing, Paused");
            Console.WriteLine("  -subete        : 全データをjsonで表示");
            Console.WriteLine("  -? or --herupu : このヘルプメッセージを表示");
            return;
        }

        var sesshonIchiran = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        var sesshon = sesshonIchiran.GetCurrentSession();
        if (sesshon == null)
        {
            Console.WriteLine("アクティブなメディアセッションがありません");
            return;
        }

        var puropati = await sesshon.TryGetMediaPropertiesAsync();
        string taitoru = puropati.Title;
        string aatisuto = puropati.Artist;

        var saiseiJouhou = sesshon.GetPlaybackInfo();
        var taimurainPuropati = sesshon.GetTimelineProperties();

        double genzaiIchi = taimurainPuropati.Position.TotalSeconds;
        double kaishiJikan = taimurainPuropati.StartTime.TotalSeconds;
        double shuuryouJikan = taimurainPuropati.EndTime.TotalSeconds;

        string saiseiJoutai = saiseiJouhou.PlaybackStatus.ToString();

        switch (hikisuu[0].ToLower())
        {
            case "-namae":
                Console.WriteLine(taitoru);
                break;
            case "-aatisuto":
                Console.WriteLine(aatisuto);
                break;
            case "-setsumei":
                Console.WriteLine($"{taitoru} - {aatisuto}");
                break;
            case "-aikon":
                if (puropati.Thumbnail != null)
                {
                    string? base64Samuneiru = null;
                    if (puropati.Thumbnail != null)
                    {
                        using var sutoriimu = await puropati.Thumbnail.OpenReadAsync();
                        using var memoriSutoriimu = new MemoryStream();
                        await sutoriimu.AsStreamForRead().CopyToAsync(memoriSutoriimu);
                        base64Samuneiru = Convert.ToBase64String(memoriSutoriimu.ToArray());
                    }

                    Console.WriteLine(base64Samuneiru);
                }
                else
                {
                    Console.WriteLine("サムネイルがありません");
                }
                break;
            case "-jikan":
                var jikanDeeta = new
                {
                    kaishiJikan = kaishiJikan,
                    shuuryouJikan = shuuryouJikan,
                    keikaJikan = genzaiIchi
                };

                Console.WriteLine(JsonSerializer.Serialize(jikanDeeta));

                break;
            case "-joutai":
                Console.WriteLine(saiseiJoutai);
                break;
            case "-subete":
                {
                    string? base64Samuneiru = null;
                    if (puropati.Thumbnail != null)
                    {
                        using var sutoriimu = await puropati.Thumbnail.OpenReadAsync();
                        using var memoriSutoriimu = new MemoryStream();
                        await sutoriimu.AsStreamForRead().CopyToAsync(memoriSutoriimu);
                        base64Samuneiru = Convert.ToBase64String(memoriSutoriimu.ToArray());
                    }

                    var zenDeeta = new
                    {
                        taitoru = taitoru,
                        aatisuto = aatisuto,
                        setsumei = $"{aatisuto} - {taitoru}",
                        kaishiJikan = kaishiJikan,
                        shuuryouJikan = shuuryouJikan,
                        keikaJikan = genzaiIchi,
                        joutai = saiseiJoutai,
                        samuneiruBase64 = base64Samuneiru
                    };

                    Console.WriteLine(JsonSerializer.Serialize(zenDeeta));
                    break;
                }
            default:
                Console.WriteLine("不明な引数です");
                break;
        }
    }
}
