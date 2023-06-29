using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Core;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoSearchHistoryViewModel : NicoViewModel
    {
        public NicoSearchHistoryViewModel(INicoSearchHistoryParentViewModel parent, NicoSearchHistoryModel m)
        {
            Parent = parent;

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Word = m.Word;
            }, nameof(Word), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Type = m.Type;
            }, nameof(Type), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Date = m.Date;
            }, nameof(Date), true);

            Loaded = RelayCommand.Create(async _ =>
            {
                Display = await GetDisplay();
            });

            AddDisposed((sender, e) =>
            {
                Loaded.Dispose();
                OnDelete.TryDispose();
                OnDoubleClick.TryDispose();
                OnFavoriteAdd.TryDispose();
                OnFavoriteDel.TryDispose();
                OnKeyDown.TryDispose();

                Display.TryDispose();
            });
        }

        public INicoSearchHistoryParentViewModel Parent { get; private set; }

        public string Word
        {
            get => _Word;
            set => SetProperty(ref _Word, value);
        }
        private string _Word;

        public NicoSearchType Type
        {
            get => _Type;
            set => SetProperty(ref _Type, value);
        }
        private NicoSearchType _Type;

        public DateTime Date
        {
            get => _Date;
            set => SetProperty(ref _Date, value);
        }
        private DateTime _Date;

        public object Display
        {
            get => _Display;
            set => SetProperty(ref _Display, value);
        }
        private object _Display;

        private async Task<object> GetDisplay()
        {
            switch (Type)
            {
                case NicoSearchType.User:
                    return new NicoUserViewModel(new NicoUserModel(Word, null, null, null));
                case NicoSearchType.Mylist:
                    return new NicoMylistViewModel(await NicoMylistModel.GetNicoMylistModel(Word));
                default:
                    return Word;
            }
        }

        public ICommand OnDelete => _OnDelete = _OnDelete ?? RelayCommand.Create(_ =>
        {
            Parent.NicoSearchHistoryOnDelete(this);
        });
        private ICommand _OnDelete;

        public ICommand OnDoubleClick => _OnDoubleClick = _OnDoubleClick ?? RelayCommand.Create(_ =>
        {
            Parent.NicoSearchHistoryOnDoubleClick(this);
        });
        private ICommand _OnDoubleClick;

        public ICommand OnKeyDown => _OnKeyDown = _OnKeyDown ?? RelayCommand.Create<KeyEventArgs>(e =>
        {
            if (e.Key == Key.Enter)
            {
                OnDoubleClick.Execute(null);
            }
        });
        private ICommand _OnKeyDown;

        public ICommand OnFavoriteAdd => _OnFavoriteAdd = _OnFavoriteAdd ?? RelayCommand.Create(_ =>
        {
            NicoModel.AddSearchFavorite(Word, Type);
        });
        private ICommand _OnFavoriteAdd;

        public ICommand OnFavoriteDel => _OnFavoriteDel = _OnFavoriteDel ?? RelayCommand.Create(_ =>
        {
            NicoModel.DelSearchFavorite(Word, Type);
        });
        private ICommand _OnFavoriteDel;

        public override IRelayCommand Loaded { get; }

    }
}