namespace EserS5L4.Models
{
    public class Item
    {
        // Campi
        private int _id;
        private double _price;
        private string _name;
        private string _description;
        //private List<string> _imageUrls = new List<string>(3);
        private string[] _imageUrls = new string[3];
        private string notFoundImg = "https://salonlfc.com/wp-content/uploads/2018/01/image-not-found-scaled-1150x647.png";

        // Proprietà (putroppo ho dovuto mettere i set per permettere il bind usando "asp-for", altrimenti avrei lasciato solo get
        public int Id { get { return _id; } set { _id = value; } }
        public double Price { get { return _price; } set { _price = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        //public List<string> ImageUrls { get { return _imageUrls; } }
        public string[] ImageUrls { get { return _imageUrls; } }

        // utile per il soft delete
        public DateTime? DeletedAt { get; set; } = null;

        // Costruttori

        // inizializza un oggetto che ha già tutte e tre le img con un link di default
        public Item(int id, double price, string name, string description)
        {
            _id = id;
            _price = price;
            _name = name;
            _description = description;

            for (int i = 0; i < _imageUrls.Length; i++) { _imageUrls[i] = notFoundImg; }
        }

        // questo conviene rifarlo mettendo tutte e tre le stringhe come default oppure usando i metodi se l'utente le modifica
        public Item(int id, double price, string name, string description, string[] imageUrls)
        {
            _id = id;
            _price = price;
            _name = name;
            _description = description;
            _imageUrls = imageUrls;
        }

        // Questo costruttore vuoto serve ad ASP.NET per creare una istanza del mio item per far bindare bene il Model!
        // altrimenti dà errore
        public Item() { }

        // Metodi della classe
        public void UpdateItemData(string newName, double newPrice, string newDescription)
        {
            _name = newName;
            _price = newPrice;
            _description = newDescription;
        }

        // Avendo solo 3 immagini, vado a modificarle una alla volta nella posizione specifica tramite metodi
        // forse potevo semplicemente fare un unico metodo e passargli anche l'id... 
        public void UpdateMainImageUrl(string imageUrl)
        {
            _imageUrls[0] = imageUrl;
        }

        public void UpdateSecondaryImageUrl(string imageUrl)
        {
            _imageUrls[1] = imageUrl;
        }

        public void UpdateTertiaryImageUrl(string imageUrl)
        {
            _imageUrls[2] = imageUrl;
        }
    }
}
