using PokeGUI.Services;
using PokeGUI.Models;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using guiWapper1;
using System.Windows;
using Prism.Commands;

namespace PokeGUI.ViewModels
{
    public class PokedexViewModel : BindableDataErrorInfoBase
    {
        private readonly IPokemonRegistry pokemonRegistry;
        private readonly PokeTypeRegistry pokeTypeRegistry;
        private readonly IPokePdfService pokePdfService;
        private readonly IPokemonExcelService pokemonExcelService;

        public PokedexViewModel(IPokemonRegistry pokemonRegistry, 
                                PokeTypeRegistry pokeTypeRegistry,
                                IPokePdfService pokePdfService,
                                IPokemonExcelService pokemonExcelService)
        {
            this.pokemonRegistry = pokemonRegistry;
            this.pokeTypeRegistry = pokeTypeRegistry;
            this.pokePdfService = pokePdfService;
            this.pokemonExcelService = pokemonExcelService;
            PokeTypeErrorVisibility = Visibility.Collapsed;
            LoadPokemonTask = LoadAsync();
        }
        
        public Task LoadAsync()
        {
            LoadingListVisibility = Visibility.Visible;
            GridVisibility = Visibility.Visible;
            return Task.Run( async () =>
            {
                PokemonCollection = new List<Pokemon>(await pokemonRegistry.GetAllPokemonAsync());
                LoadingListVisibility = Visibility.Collapsed;
                PokeTypes = pokeTypeRegistry.All();
                SelectedPokeType = pokeTypeRegistry.None;
            });

        }

        private Task loadPokemonTask;
        public Task LoadPokemonTask
        {
            get { return loadPokemonTask; }
            set {
                SetProperty(ref loadPokemonTask, value); 
            }
        }

        private List<PokeType> pokeTypes;
        public List<PokeType> PokeTypes
        {
            get { return pokeTypes; }
            set {
                SetProperty(ref pokeTypes, value); 
            }
        }


        private string pokemonNameFilter;
        public string PokemonNameFilter
        {
            get => pokemonNameFilter;
            set
            {
                SetProperty(ref pokemonNameFilter, value);
                RaisePropertyChanged(nameof(PokemonFilteredCollection));
                if (value.Contains(" "))
                {
                    NameError = "Name cannot have a space";
                }
                else if (PokemonFilteredCollection.Count <= 0)
                {
                    NameError = "There isn't a pokemon with these search values in your list";
                }
                else
                {
                    NameError = null;
                }
            }
        }

        private string nameError;
        public string NameError
        {
            get { return nameError; }
            set
            {
                SetProperty(ref nameError, value);
                ErrorDictionary[nameof(PokemonNameFilter)] = value;
                nameErrorVisibility = value?.Length > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private string pokeTypeError;
        public string PokeTypeError
        {
            get { return pokeTypeError; }
            set 
            {
                SetProperty(ref pokeTypeError, value);
                ErrorDictionary[nameof(LoadFilter)] = value;
                pokeTypeErrorVisibility = value?.Length > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private Visibility pokeTypeErrorVisibility;
        public Visibility PokeTypeErrorVisibility
        {
            get { return pokeTypeErrorVisibility; }
            set { SetProperty(ref pokeTypeErrorVisibility, value); }
        }


        private Visibility nameErrorVisibility;
        public Visibility NameErrorVisibility
        {
            get { return nameErrorVisibility; }
            set { SetProperty(ref nameErrorVisibility, value); }
        }

        private Visibility gridVisibility;
        public Visibility GridVisibility
        {
            get { return gridVisibility; }
            set { SetProperty(ref gridVisibility, value); }
        }

        private Visibility loadingListVisibility;
        public Visibility LoadingListVisibility
        {
            get { return loadingListVisibility; }
            set
            {
                SetProperty(ref loadingListVisibility, value);
                //GridVisibility = (loadingListVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private List<Pokemon> pokemonCollection;
        public List<Pokemon> PokemonCollection
        {
            get { return pokemonCollection; }
            set { 
                SetProperty(ref pokemonCollection, value);
                RaisePropertyChanged(nameof(PokemonFilteredCollection));
            }
        }

        private PokeType selectedPokeType;

        public PokeType SelectedPokeType
        {
            get { return selectedPokeType; }
            set {
                SetProperty(ref selectedPokeType, value);
                RaisePropertyChanged(nameof(PokemonFilteredCollection));
                if (PokemonFilteredCollection.Count <= 0)
                {
                    NameError = "There isn't a pokemon with these search values in your list";
                }
                else
                {
                    NameError = null;
                }
            }
        }

        public ObservableCollection<Pokemon> PokemonFilteredCollection
        {
            get {
                var list1 = FilterPokemonByType();
                var list2 = FilterPokemonByName(list1);
                return new ObservableCollection<Pokemon>(list2 ?? new List<Pokemon>()); 
            }
        }

        public IEnumerable<Pokemon> FilterPokemonByName(List<Pokemon> pokeList)
        {
            return string.IsNullOrEmpty(PokemonNameFilter) == false
                ? pokeList.FindAll(p => p.Name.StartsWith(PokemonNameFilter))
                : pokeList;
        }
        public List<Pokemon> FilterPokemonByType()
        {
            if (PokemonCollection != null)
            {
                return SelectedPokeType != null
                   ? PokemonCollection.FindAll(p =>
                           p.Type1.TypeName == SelectedPokeType.TypeName
                           || (p.Type2 != null ? p.Type2.TypeName == SelectedPokeType.TypeName : false)
                           || SelectedPokeType.TypeName == "none")
                   : PokemonCollection;
            } 
            else
            {
                return new List<Pokemon>();
            }
        }

        private DelegateCommand printPokemon;

        public DelegateCommand PrintPokemon => printPokemon ?? (printPokemon = new DelegateCommand(() =>
        {
            pokePdfService.WritePdf(PokemonFilteredCollection);
        }));

        private DelegateCommand loadFilter;

        public DelegateCommand LoadFilter => loadFilter ?? (loadFilter = new DelegateCommand(() => 
        {
            (PokemonNameFilter, SelectedPokeType) =  pokemonExcelService.getStoredFilter();

            //View expects selected types from poketypes
            SelectedPokeType = PokeTypes.Find(t => t.TypeName == SelectedPokeType.TypeName);

            PokeTypeError = SelectedPokeType == null
                ? "Selected Pokemon type is not supported"
                : null;
        }));

        private DelegateCommand saveExcel;

        public DelegateCommand SaveExcel => saveExcel ?? (saveExcel = new DelegateCommand(() =>
        {
            pokemonExcelService.generatePokemonExcelSheet(PokemonFilteredCollection);
        }));


    }
}
