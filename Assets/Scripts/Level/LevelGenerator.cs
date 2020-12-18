using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


namespace HackedDesign
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] GameObject parent;

        [Header("Prefabs")]
        [SerializeField] List<Section> startPrefabs;
        [SerializeField] List<Section> endPrefabs;
        [SerializeField] List<Section> sectionPrefabs;

        public void Generate(int length)
        {
            Logger.Log(this, "Generating level");
            Logger.Log(this, "Length: ", length.ToString());
            DestroyLevel();
            var remainingLength = length;
            //Bounds bounds = new Bounds();
            var section = SpawnSection(startPrefabs, parent.transform.position, remainingLength);

            //bounds.Encapsulate(section.gameObject.GetComponent<Renderer>().bounds);


            remainingLength -= section.length;

            while (remainingLength > 1)
            {
                var start = section.exit.transform.position;
                section = SpawnSection(sectionPrefabs, start, remainingLength);
                //bounds.Encapsulate(section.gameObject.GetComponent<Renderer>().bounds);

                remainingLength -= section.length;

                //SpawnBarrier(sectionTimeBarrier, section.length, false, section.exit.transform.position);

                Logger.Log(this, "Remaining Length:", remainingLength.ToString());
            }

            var endpos = section.exit.transform.position;

            section = SpawnSection(endPrefabs, endpos, 1);
            //bounds.Encapsulate(section.gameObject.GetComponent<Renderer>().bounds);

        }

        public void DestroyLevel()
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
                GameObject.Destroy(parent.transform.GetChild(i).gameObject);
            }
        }

        public Section SpawnSection(List<Section> sectionPrefabList, Vector3 position, int remainingLength)
        {
            var available = sectionPrefabList.Where(s => s.length <= remainingLength).ToList();

            int index = Random.Range(0, available.Count());

            var sectionObj = GameObject.Instantiate(available[index].gameObject, position, Quaternion.identity, parent.transform);
            return sectionObj.GetComponent<Section>();
        }
    }
}