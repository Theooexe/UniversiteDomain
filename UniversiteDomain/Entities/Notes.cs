namespace UniversiteDomain.Entities
{
    public class Notes
    {
        public float Valeur { get; set; }
        
        public long EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }  

        // Association avec l'Ue
        public long UeId { get; set; }
        public Ue Ue { get; set; }  // Navigation vers l'entité Ue

        
    }
}