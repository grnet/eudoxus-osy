namespace EudoxusOsy.BusinessModel
{
	public enum enBookType
	{
        // ����-������ -- ���� ��� ����� ���
		SelfPublished = 0,
        // Published ��� ��� ��� regular ��� ������ ���
		Regular = 1,

        // ePublished ��� ��� ��� ePublished ��� ������ ���, �� e-Published ����� ���� ��� ��������� 
        // ��������������-����������� �� ��� ������ ���� (��. ������� ���.)
		EPublished = 2,

        // ProfessorNotes ��� ���
        ProfessorNotes = 3,

        // eBook ��� ���
        eBook = 4,

        // ����
        Other = 5
	}
}