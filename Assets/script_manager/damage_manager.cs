using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �ڷ� ��ó https://glikmakesworld.tistory.com/2
// �̱������� �����, MonoBehaviour�� ��� ���� �ʾ� ���� �Ѿ�� �����Ҽ� �ְ� �����.(��� ȭ��� ������ �ʴ´�.)
public class damage_manager
{
    private static damage_manager instance;

    public static damage_manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new damage_manager();
            }
            return instance;
        }
    }
    //����
    public damage_manager()
    {
        
    }
    public int en_dmg = 1; //�� ���ݷ��� ���� ����(ui ������ ������ �Ѵ�.)
    public void damage_count(int enemy_damage)
    {
        en_dmg = enemy_damage;
    }
}
