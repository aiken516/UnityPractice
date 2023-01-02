using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
//

public class Character : MonoBehaviour
{
    SpriteRenderer SR;
    [SerializeField] public CharacterSO characterSO;

    Tile[,] Tiles;

    #region Loc X, Y
    [SerializeField] int locX, locY;
    public int LocX => locX;
    public int LocY => locY;
    #endregion
    float MaxHP, CurHP;


    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        Tiles = GameManager.Instance.BattleMNG.BattleField.TileArray;

        Init();
        SetLotate();
    }

    // �ν����Ϳ��� ��ġ �̵���Ű�� ���� �ӽ÷� ��ġ
    private void Update()
    {
        SetLotate();
    }

    // ĳ���� �ʱ�ȭ
    void Init()
    {
        GameManager.Instance.BattleMNG.CharEnter(GetComponent<Character>());

        SR.flipX = (characterSO.team == Team.Enemy) ? true : false;
        MaxHP = CurHP = characterSO.stat.HP;
    }

    // ��ų ���
    public void use()
    {
        StartCoroutine(CoUse());
    }
    IEnumerator CoUse()
    {
        characterSO.use(GetComponent<Character>());

        yield return new WaitForSeconds(0.5f);
    }

    public void MoveLotate(int x, int y)
    {
        Tiles[LocY, LocX].ExitTile();

        int dumpX = locX;
        int dumpY = locY;

        if(0 <= locX + x && locX + x < 8)
            dumpX += x;
        if (0 <= locY + y && locY + y < 3)
            dumpY += y;

        if(!Tiles[dumpY, dumpX].isOnTile)
        {
            locX = dumpX;
            locY = dumpY;
        }


        SetLotate();
    }

    public void SetLotate()
    {
        Vector3 vec = GameManager.Instance.BattleMNG.BattleField.GetTileLocate(LocX, LocY);
        transform.position = vec;

        Tiles[LocY, LocX].EnterTile(GetComponent<Character>());
    }

    public void GetDamage(float DMG)
    {
        CurHP -= DMG;
        Debug.Log("DMG : " + DMG + ", CurHP ; " + CurHP);
        DeadCheck();
    }

    void DeadCheck()
    {
        if(CurHP <= 0)
        {
            // �׾��� �� ó��
            GameManager.Instance.BattleMNG.CharExit(GetComponent<Character>());
            Tiles[LocY, LocX].ExitTile();

            Destroy(gameObject);
        }
    }

    public int GetSpeed() => characterSO.stat.SPD;
}
