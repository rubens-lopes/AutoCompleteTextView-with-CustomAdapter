using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TextView = Android.Support.V7.Widget.AppCompatTextView;

namespace AutoComplete
{
    public class SongAdapter : BaseAdapter<Song>, IFilterable
    {
        List<Song> _songs;
        protected List<Song> _allSongs;
        SongFilter _filter;

        public SongAdapter(ICollection<Song> songs)
        {
            _songs = songs.ToList();
            _allSongs = new List<Song>(songs);
            _filter = new SongFilter(this);
        }

        public override Song this[int position]
        {
            get
            {
                return _songs != null ? _songs.ElementAt(position) : new Song();
            }
        }
        public override int Count
        {
            get
            {
                return _songs != null ? _songs.Count : 0;
            }
        }
        public Filter Filter
        {
            get
            {
                return _filter != null ? _filter : new SongFilter(this);
            }
        }
        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var song = _songs[position];
            View view = convertView;

            if (view == null)
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SongListItem, parent, false);

            view.FindViewById<TextView>(Resource.Id.Name).Text = song.Name;
            view.FindViewById<TextView>(Resource.Id.Artist).Text = song.Artist;

            return view;
        }

        protected void Clear()
        {
            _songs.Clear();
        }

        protected void Add(Song song)
        {
            _songs.Add(song);
        }

        public class SongFilter : Filter
        {
            JavaList<Song> _suggestions;
            readonly SongAdapter _adapter;

            public SongFilter(SongAdapter adapter)
            {
                _adapter = adapter;
                _suggestions = new JavaList<Song>();
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                if (constraint != null)
                {
                    var terms = (constraint.ToString() as string);

                    _suggestions.Clear();
                    foreach (var song in _adapter._allSongs)
                        if (Regex.IsMatch(song.ToString(), @"(?<!@)(\b" + terms + ")", RegexOptions.IgnoreCase))
                            _suggestions.Add(song);

                    FilterResults filterResults = new FilterResults();
                    filterResults.Values = _suggestions;
                    filterResults.Count = _suggestions.Count;
                    return filterResults;
                }
                else
                    return new FilterResults();
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                _adapter.Clear();
                var songs = results.Values as JavaList<Song>;

                if (songs != null)
                    foreach (var song in songs)
                        _adapter.Add(song);
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}