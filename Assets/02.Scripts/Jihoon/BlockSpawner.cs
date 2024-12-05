using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blockPrefabs;   // ���� ��� ������ �迭
    public int width = 10;              // ���� ĭ ��
    public int height = 10;             // ���� ĭ ��
    public int depth = 3;               // ���� �� �� (����)
    public int totalBlocks = 50;        // ������ ��ü ��� ��

    private bool[,,] occupied;          // ��� ��ġ�� �����Ǿ����� �����ϴ� 3D �迭
    private List<Vector3Int> directions = new List<Vector3Int> {
        Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down, Vector3Int.forward, Vector3Int.back
    };

    private float blockScale = 0.1f; // ����� �������� 0.1�̹Ƿ� ������ ���߱� ���� ����

    void Start()
    {
        occupied = new bool[width, height, depth];  // ���� ���� �迭 �ʱ�ȭ
        GenerateBlockStructure();
    }

    public void GenerateBlockStructure()
    {
        // occupied �迭 �ʱ�ȭ
        occupied = new bool[width, height, depth];

        // ��� �׷� ����
        GameObject blockGroup = new GameObject("BlockGroup");
        BlockGroup groupComponent = blockGroup.AddComponent<BlockGroup>(); // �׷��� ������Ʈ �߰�

        // ù ��° ��� ����
        Vector3Int start = new Vector3Int(width / 2, 0, depth / 2);
        occupied[start.x, start.y, start.z] = true;
        InstantiateRandomBlock(start, blockGroup.transform);

        List<Vector3Int> blockPositions = new List<Vector3Int> { start };

        for (int i = 1; i < totalBlocks; i++)
        {
            Vector3Int currentPos = blockPositions[Random.Range(0, blockPositions.Count)];
            Vector3Int newPos = currentPos + directions[Random.Range(0, directions.Count)];

            if (IsValidPosition(newPos) && IsExposed(newPos))
            {
                occupied[newPos.x, newPos.y, newPos.z] = true;
                blockPositions.Add(newPos);
                InstantiateRandomBlock(newPos, blockGroup.transform);
            }
        }
    }

    void InstantiateRandomBlock(Vector3Int position, Transform parent)
    {
        GameObject selectedPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Length)];
        Vector3 spawnerPosition = transform.position;
        Vector3 actualPosition = spawnerPosition + new Vector3(position.x, position.y, position.z) * blockScale;

        // ����� �ν��Ͻ�ȭ�ϰ�, ��� �׷��� �ڽ����� ����
        GameObject newBlock = Instantiate(selectedPrefab, actualPosition, Quaternion.identity, parent);

        // ��Ͽ� ������ٵ� ������ �߰�
        Rigidbody blockRigidbody = newBlock.GetComponent<Rigidbody>();
        if (blockRigidbody == null)
        {
            blockRigidbody = newBlock.AddComponent<Rigidbody>();
        }

        // BlockGroup�� ����Ʈ�� ������ٵ� �߰�
        BlockGroup groupComponent = parent.GetComponent<BlockGroup>();
        groupComponent.blocksInGroup.Add(blockRigidbody);
    }

    bool IsValidPosition(Vector3Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height || pos.z < 0 || pos.z >= depth)
            return false;

        return !occupied[pos.x, pos.y, pos.z];
    }

    bool IsExposed(Vector3Int pos)
    {
        foreach (Vector3Int dir in directions)
        {
            Vector3Int neighbor = pos + dir;
            if (neighbor.x >= 0 && neighbor.x < width &&
                neighbor.y >= 0 && neighbor.y < height &&
                neighbor.z >= 0 && neighbor.z < depth &&
                !occupied[neighbor.x, neighbor.y, neighbor.z])
            {
                return true;
            }
        }
        return false;
    }
}
