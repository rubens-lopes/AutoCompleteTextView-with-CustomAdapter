using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using AutoCompleteTextView = Android.Support.V7.Widget.AppCompatAutoCompleteTextView;
using Button = Android.Support.V7.Widget.AppCompatButton;

namespace AutoComplete
{
    [Activity(Label = "AutoComplete", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            using (var reader = new StreamReader(Assets.Open("songs.json")))
            {
                var autoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);
                var songs = JsonConvert.DeserializeObject<ICollection<Song>>(reader.ReadToEnd());

                autoComplete.Adapter = new SongAdapter(songs);
            }

            var bottomSheet = FindViewById(Resource.Id.bottomSheet);
            var buttonEmbed = FindViewById<Button>(Resource.Id.buttonEmbed);
            var buttonFragment = FindViewById<Button>(Resource.Id.buttonFragment);

            var bottomSheetBehavior = BottomSheetBehavior.From(bottomSheet);

            buttonEmbed.Click += delegate
            {
                bottomSheetBehavior.State = BottomSheetBehavior.StateExpanded;
            };

            buttonFragment.Click += delegate
            {
                var searchMusicFragment = new SearchMusicFragment();
                searchMusicFragment.Show(SupportFragmentManager, searchMusicFragment.Tag);
            };
        }
    }
}

