namespace EudoxusOsy.BusinessModel
{
	public enum enBookType
	{
        // Αυτο-έκδοση -- μόνο στο παλιό ΟΣΥ
		SelfPublished = 0,
        // Published του ΚΠΣ και regular του παλιού ΟΣΥ
		Regular = 1,

        // ePublished του ΚΠΣ και ePublished του παλιού ΟΣΥ, τα e-Published είναι αυτά που παλιότερα 
        // κοστολογούνταν-πληρώνονταν με τον ειδικό τύπο (αρ. σελίδων κλπ.)
		EPublished = 2,

        // ProfessorNotes του ΚΠΣ
        ProfessorNotes = 3,

        // eBook του ΚΠΣ
        eBook = 4,

        // άλλο
        Other = 5
	}
}