using Newtonsoft.Json;

namespace HistoryToEndsong
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter directory: ");
            string directory = Console.ReadLine()!;

            while (true)
            {
                ConvertFile(directory);

                Console.Write("Press enter to convert another file...");
                string another = Console.ReadLine()!;
                if (another != "")
                    break;
            }
        }

        public static void ConvertFile(string directory) 
        {
            Console.Write("Enter start year: ");
            string startYear = Console.ReadLine()!;

            Console.Write("Enter end year: ");
            string endYear = Console.ReadLine()!;

            Console.Write("Enter file number: ");
            int currentFile = int.Parse(Console.ReadLine()!);

            string filename = $"Streaming_History_Audio_{startYear}-{endYear}_{currentFile}.json";
            if (startYear == endYear)
                filename = $"Streaming_History_Audio_{startYear}_{currentFile}.json";

            string path = $"{directory}\\{filename}";
            Console.WriteLine($"Reading in file at {path}...");
            try
            {
                string json = File.ReadAllText(path);
                StreamingHistoryTrack[] tracks = JsonConvert.DeserializeObject<StreamingHistoryTrack[]>(json)!;
                Console.WriteLine($"Read in {tracks.Length} tracks.");

                Console.WriteLine($"Converting to endsong_{currentFile}.json...");
                List<EndsongTrack> endsongTracks = new List<EndsongTrack>();
                foreach (StreamingHistoryTrack track in tracks)
                {
                    endsongTracks.Add(new EndsongTrack
                    {
                        albumName = track.master_metadata_album_album_name,
                        artistName = track.master_metadata_album_artist_name,
                        trackName = track.master_metadata_track_name,
                        datetime = track.ts
                    });
                }

                Console.WriteLine($"Serialising JSON data...");
                string endsongJson = JsonConvert.SerializeObject(endsongTracks);
                Console.WriteLine($"Writing to endsong_{currentFile}.json...");
                File.WriteAllText($"{directory}\\endsong_{currentFile}.json", endsongJson);
                Console.WriteLine($"Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading file: {e.Message}");
            }
        }
    }

    public class StreamingHistoryTrack
    {
        public string ts { get; set; }
        public string username { get; set; }
        public string platform { get; set; }
        public int ms_played { get; set; }
        public string conn_country { get; set; }
        public string ip_addr_decrypted { get; set; }
        public string user_agent_decrypted { get; set; }
        public string master_metadata_track_name { get; set; }
        public string master_metadata_album_artist_name { get; set; }
        public string master_metadata_album_album_name { get; set; }
        public string spotify_track_uri { get; set; }
        public string episode_name { get; set; }
        public string episode_show_name { get; set; }
        public string spotify_episode_uri { get; set; }
        public string reason_start { get; set; }
        public string reason_end { get; set; }
        public bool shuffle { get; set; }
        public string skipped { get; set; }
        public bool offline { get; set; }
        public string offline_timestamp { get; set; }
        public bool incognito_mode { get; set; }
    }

    public class EndsongTrack 
    {
        public string albumName { get; set; }
        public string artistName { get; set; }
        public string trackName { get; set; }
        public string datetime { get; set; }
    }
}
