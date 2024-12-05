using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blockPrefabs;   // 여러 블록 프리팹 배열
    public int width = 10;              // 가로 칸 수
    public int height = 10;             // 세로 칸 수
    public int depth = 3;               // 쌓을 층 수 (높이)
    public int totalBlocks = 50;        // 생성할 전체 블록 수

    private bool[,,] occupied;          // 블록 위치가 점유되었는지 저장하는 3D 배열
    private List<Vector3Int> directions = new List<Vector3Int> {
        Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down, Vector3Int.forward, Vector3Int.back
    };

    private float blockScale = 0.1f; // 블록의 스케일이 0.1이므로 간격을 맞추기 위해 설정

    void Start()
    {
        occupied = new bool[width, height, depth];  // 점유 상태 배열 초기화
        GenerateBlockStructure();
    }

    public void GenerateBlockStructure()
    {
        // occupied 배열 초기화
        occupied = new bool[width, height, depth];

        // 블록 그룹 생성
        GameObject blockGroup = new GameObject("BlockGroup");
        BlockGroup groupComponent = blockGroup.AddComponent<BlockGroup>(); // 그룹의 컴포넌트 추가

        // 첫 번째 블록 생성
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

        // 블록을 인스턴스화하고, 블록 그룹의 자식으로 설정
        GameObject newBlock = Instantiate(selectedPrefab, actualPosition, Quaternion.identity, parent);

        // 블록에 리지드바디가 없으면 추가
        Rigidbody blockRigidbody = newBlock.GetComponent<Rigidbody>();
        if (blockRigidbody == null)
        {
            blockRigidbody = newBlock.AddComponent<Rigidbody>();
        }

        // BlockGroup의 리스트에 리지드바디 추가
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
