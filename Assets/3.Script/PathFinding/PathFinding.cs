using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector3 position;
    public int f, g, h;
    public bool closed = false;
    public bool canMove = false;
    public Vector3 hangDownArrowPos = Vector3.zero;

    public Node preNode;

    public Node(Vector3 position, Vector3 hangDownArrowPos, bool canMove)
    {
        this.position = position;
        this.hangDownArrowPos = hangDownArrowPos;
        this.canMove = canMove;
    }

    public int CompareTo(Node other)
    {
        return f > other.f ? 1 : -1;
    }
}

public class PathFinding : MonoBehaviour, ISceneContextBuilt
{
    [SerializeField]
    private Transform leftDown, rightUp;
    [SerializeField]
    private Transform origin;
    [SerializeField]
    private Transform destination;
    [SerializeField]
    private GameObject tilePrefab;

    [Header("Setting")]
    [SerializeField]
    private LayerMask canMovingLayer;

    private Vector3 destinationVector;
    private int row;
    private List<Node> nodes = new List<Node>();
    private Node endNode;

    Coroutine huntingCoroutine = null;

    public int Priority { get; set; } = 1;

    public void OnSceneContextBuilt()
    {
        row = (int)rightUp.position.x - (int)leftDown.position.x + 1;

        StartCoroutine(AutoHunting_co());
    }

    public IEnumerator AutoHunting_co()
    {
        PlayerCharacter pc = GameManager.Instance.CurrentSceneContext.PlayerCharacter;
        while(true)
        {
            yield return null;

            Debug.Log("0000");
            if (pc.IsAuto)
            {
                if(huntingCoroutine == null)
                {
                    bool isDead = origin.gameObject.activeSelf;
                    Debug.Log(origin.gameObject.name + " " + origin.gameObject.activeSelf);
                    if (isDead)
                    {
                        Debug.Log("1111");
                        huntingCoroutine = StartCoroutine(Hunting_co());
                    }
                }
            }
            else if(!pc.IsAuto && huntingCoroutine != null)
            {
                Debug.Log("3333");
                StopCoroutine(huntingCoroutine);
            }
        }
    }

    private void Detectnodes()
    {
        Color[] colors = new Color[3] { Color.red, Color.blue, Color.yellow };

        for (int i = (int)leftDown.position.y; i <= rightUp.position.y; i++)
        {
            for (int j = (int)leftDown.position.x; j <= rightUp.position.x; j++)
            {
                RaycastHit2D[] hit2D = Physics2D.CircleCastAll(new Vector3(j, i), 0.4f, Vector3.up, 0f, canMovingLayer.value);
                Vector3 pos = new Vector3(j, i);
                Vector3 hangDownArrowPos = Vector3.zero;
                bool canMove = false;

                foreach (var v in hit2D)
                {
                    //SpriteRenderer sp = Instantiate(tilePrefab, new Vector3(j, i), Quaternion.identity).GetComponent<SpriteRenderer>();
                    //sp.color = colors[Mathf.Abs(j % 2)];
                    canMove = true;
                    //
                    if (v.collider.gameObject.layer == LayerMask.NameToLayer("Hangable"))
                    {
                        RaycastHit2D hit;
                        hit = Physics2D.Raycast(v.point, Vector2.down, 5f, 1 << LayerMask.NameToLayer("Foothold"));
                        hangDownArrowPos = hit.point;
                        //sp.color = Color.yellow;
                        break;
                    }
                }

                nodes.Add(new Node(pos, hangDownArrowPos, canMove));
            }
        }
    }


    private IEnumerator Hunting_co()
    {
        endNode = null;
        nodes.Clear();
        Detectnodes();

        destinationVector = destination.position;
        destinationVector.x = (int)destinationVector.x;
        destinationVector.y = (int)destinationVector.y;
        endNode = null;
        PriorityQueue<Node> queue = new PriorityQueue<Node>();
        int[] dx = new int[] { 0, 0, -1, 1 };
        int[] dy = new int[] { 1, -1, 0, 0 };

        queue.Enqueue(GetNode((int)origin.position.x, (int)origin.position.y));
        while (queue.Count != 0)
        {
            Node curNode = queue.Dequeue();
            curNode.closed = true;

            if (IsNearby(curNode.position, destinationVector))
            {
                endNode = curNode;
                break;
            }
            //SpriteRenderer sp = Instantiate(tilePrefab, curNode.position, Quaternion.identity).GetComponent<SpriteRenderer>();
            //sp.color = Color.red;
            //yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < 4; i++)
            {
                int newPosX = (int)curNode.position.x + dx[i];
                int newPosY = (int)curNode.position.y + dy[i];

                Node checkNode = GetNode(newPosX, newPosY);
                if (checkNode != null && !checkNode.closed && checkNode.canMove)
                {

                    int weight = 10;

                    int g = curNode.g + weight;
                    int h = ((int)Mathf.Abs(destinationVector.x - newPosX) + (int)Mathf.Abs(destinationVector.y - newPosY)) * weight;
                    int f = g + h;
                    checkNode.g = g;
                    checkNode.h = h;
                    checkNode.f = -f;
                    checkNode.preNode = curNode;
                    queue.Enqueue(checkNode);
                    //SpriteRenderer sp1 = Instantiate(tilePrefab, checkNode.position, Quaternion.identity).GetComponent<SpriteRenderer>();
                    //sp1.color = Color.yellow;
                }
            }
            //yield return new WaitForSeconds(0.3f);
        }

        List<Node> road = new List<Node>();
        Node curNode1 = endNode;
        while (curNode1 != null)
        {
            road.Add(curNode1);
            curNode1 = curNode1.preNode;
        }

        road.Reverse();

        yield return ChaseAndAttack_co(road);

        huntingCoroutine = null;
    }

    private IEnumerator ChaseAndAttack_co(List<Node> road)
    {
        if (road.Count <= 0)
            yield break;

        PlayerCharacter pc = GameManager.Instance.CurrentSceneContext.PlayerCharacter;
        for (int i = 1; i < road.Count;)
        {
            int dirX = (int)road[i].position.x - (int)road[i - 1].position.x;

            if (road[i - 1].position.y == road[i].position.y)
            {
                pc.SetMoveDir(new Vector3(dirX, 0));
                pc.StateMuchine.ChangeState(new WalkState());

                yield return new WaitUntil(() => IsNearby(origin.position, road[i].position, 0.1f, false, true));
                i++;
            }
            else
            {
                if (i >= road.Count - 2)
                    break;

                int dirY = (int)road[i].position.y - (int)road[i - 1].position.y;
                pc.StateMuchine.ChangeState(new JumpState());

                yield return new WaitForSeconds(0.3f);

                Transform hangTransform = pc.CanHanging();

                if(hangTransform != null)
                {
                    pc.transform.position = new Vector3(hangTransform.position.x, pc.transform.position.y);
                    pc.StateMuchine.ChangeState(new HangState());
                }
                pc.SetMoveDir(new Vector3(0, dirY));

                bool flag = false;
                yield return new WaitWhile(() =>
                {
                    if (IsNearby(pc.transform.position, road[i].position, 0.2f))
                    {
                        //Debug.Log($"prePos : {road[i - 1].position}  curPos : {road[i].position} dir : {pc.MoveDir} state : {pc.StateMuchine.CurrentState.GetType()}");
                        flag = true;
                        i++;
                    }
                    return pc.CanHanging() != null;
                });

                pc.downArrowJump = true;
                pc.StateMuchine.ChangeState(new JumpState());

                if (!flag)
                    i++;
                yield return new WaitUntil(() => pc.StateMuchine.CurrentState.GetType() == new IdleState().GetType());
            }

            //Debug.Log($"prePos : {road[i - 1].position}  curPos : {road[i].position} dir : {pc.MoveDir} state : {pc.StateMuchine.CurrentState.GetType()}");
        }

        pc.SetMoveDir(Vector3.zero);
        pc.StateMuchine.ChangeState(new IdleState());

        pc.AttackDefaultSkill();
    }

    private Node GetNode(int x, int y)
    {
        int index = (x - (int)leftDown.position.x) + row * (y - (int)leftDown.position.y);

        if (nodes.Count > index && index >= 0)
            return nodes[index];
        else
            return null;
    }
    private bool IsNearby(Vector3 pos, Vector3 destination, float distance = 1f, bool ignoreX = false, bool ignoreY = false)
    {
        if ((ignoreX || (!ignoreX && pos.x >= destination.x - distance && pos.x <= destination.x + distance)) &&
            (ignoreY || (!ignoreY && pos.y >= destination.y - distance && pos.y <= destination.y + distance)))
            return true;

        return false;
    }

}
