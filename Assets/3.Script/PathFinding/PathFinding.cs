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

public class PathFinding : MonoBehaviour
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
    [SerializeField]
    private float detectingRadius;

    private Vector3 destinationVector;
    private int row;
    private int cal;
    private List<Node> nodes = new List<Node>();
    private Node endNode;

    private void Start()
    {
        cal = (int)rightUp.position.y - (int)leftDown.position.y + 1;
        row = (int)rightUp.position.x - (int)leftDown.position.x + 1;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(AStar());
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
                        SpriteRenderer sp = Instantiate(tilePrefab, hit.point, Quaternion.identity).GetComponent<SpriteRenderer>();
                        sp.color = Color.yellow;
                        break;
                    }
                }

                nodes.Add(new Node(pos, hangDownArrowPos, canMove));
            }
        }
    }


    private IEnumerator AStar()
    {
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
                    int g = curNode.g + 10;
                    int h = ((int)Mathf.Abs(destinationVector.x - newPosX) + (int)Mathf.Abs(destinationVector.y - newPosY)) * 10;
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

        PlayerCharacter pc = GameManager.Instance.CurrentSceneContext.PlayerCharacter;
        pc.isAuto = true;
        road.Reverse();

        Vector3 prePos = road[0].position;
        for (int i = 1; i < road.Count; i++)
        {
            Vector3 curPos = road[i].position;
            int dirX = (int)curPos.x - (int)prePos.x;

            if (prePos.y == curPos.y)
            {

                pc.SetMoveDir(new Vector3(dirX, 0));
                pc.StateMuchine.ChangeState(new WalkState());
            }
            else
            {
                int dirY = (int)curPos.y - (int)prePos.y;
                if(road[i].hangDownArrowPos != Vector3.zero)
                {
                    pc.transform.position = road[i].hangDownArrowPos;
                }
                pc.StateMuchine.ChangeState(new JumpState());

                yield return new WaitForSeconds(0.3f);

                pc.SetMoveDir(new Vector3(0, dirY));
                pc.StateMuchine.ChangeState(new HangState());

            }
            Debug.Log($"prePos : {prePos}  curPos : {curPos} dir : {pc.MoveDir} state : {pc.StateMuchine.CurrentState.GetType()}");
            yield return new WaitUntil(() => IsNearby(origin.position, curPos));
            yield return new WaitForSeconds(0.3f);
        }
        pc.isAuto = false;
    }

    private Node GetNode(int x, int y)
    {
        int index = (x - (int)leftDown.position.x) + row * (y - (int)leftDown.position.y);

        if (nodes.Count > index && index >= 0)
            return nodes[index];
        else
            return null;
    }
    private bool IsNearby(Vector3 pos, Vector3 destination)
    {
        for(int i = 0; i < 9; i++)
        {
            if (pos.x >= destination.x - 1 && pos.x <= destination.x + 1 &&
                pos.y >= destination.y - 1 && pos.y <= destination.y + 1)
                return true;
        }

        return false;
    }
}
