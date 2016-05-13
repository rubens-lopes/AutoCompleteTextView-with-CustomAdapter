using Android.App;
using Android.Support.Design.Widget;
using Android.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using AutoCompleteTextView = Android.Support.V7.Widget.AppCompatAutoCompleteTextView;

namespace AutoComplete
{
    public class SearchMusicFragment : BottomSheetDialogFragment
    {
        public override void SetupDialog(Dialog dialog, int style)
        {
            base.SetupDialog(dialog, style);
            var view = View.Inflate(Activity, Resource.Layout.SearchMusic, null);
            using (var reader = new StreamReader(Application.Context.Assets.Open("songs.json")))
            {
                var autoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);
                var songs = JsonConvert.DeserializeObject<ICollection<Song>>(reader.ReadToEnd());

                autoComplete.Adapter = new SongAdapter(songs);
            }

            dialog.SetContentView(view);

            //var layout = (CoordinatorLayout.LayoutParams)((View)view.Parent).LayoutParameters;
            //var behavior = layout.Behavior;

            //if (behavior != null)
            //    ((BottomSheetBehavior)behavior).SetBottomSheetCallback();
        }
    }
}