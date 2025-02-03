namespace UniversiteDomain.Entities
{
    public class Notes
    {
        public long Id { get; set; }

        public float Valeur { get; set; }

        // Association avec l'étudiant
        public long EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }  // Navigation vers l'entité Etudiant

       
        public long UeId { get; set; }
        public Ue Ue { get; set; }  
        
    }
}