using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    [System.Serializable]
    public class PointGoal {

        [SerializeField]
        public Vector2 position;

        [SerializeField]
        public bool    valid;

        SpriteRenderer spriteRenderer;

        public PointGoal(Vector2 sourcePos, SpriteRenderer sourceSpriteRenderer)
        {
            position = sourcePos;
            valid = false;
            spriteRenderer = sourceSpriteRenderer;            
        }

        public void updateState(bool validityState)
        {
            valid = validityState;
            if (valid)
                spriteRenderer.color = new Color(0.159f, 1, 0, 1);
            else
                spriteRenderer.color = new Color(1, 0, 0, 1);
        }

    }

    [System.Serializable]
    public class SubGoal {

        [SerializeField]
        public List<PointGoal> points;//

        [SerializeField]
        public bool            valid;

        public SubGoal(Transform baseTransform)
        {
            points = new List<PointGoal>();
            foreach (Transform child in baseTransform)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
                points.Add(new PointGoal(child.transform.position, child.GetComponent<SpriteRenderer>()));
            }

            valid = false;
        }
    };


    [SerializeField]
    public List<SubGoal> m_subGoals = new List<SubGoal>();


    private IEnumerator updateGoals()
    {
        while (true)
        {
            int nbOfValidSubGoals = 0;
            foreach (SubGoal subGoal in m_subGoals)
            {
                int nbOfValidGoals = 0;
                foreach (PointGoal pointGoal in subGoal.points)
                {
                    // Essayer OverlapCircleNonAlloc() pour optimiser les performances
                    // Il faudra passer un array de collider suffisament grand pour avoir tous les resultats possibles
                    //Collider2D[] overlaps = Physics2D.OverlapCircleAll(pointGoal.position, 0.2f);
                    Collider2D[] overlaps = Physics2D.OverlapPointAll(pointGoal.position);
                    bool check = false;
                    foreach (Collider2D overlap in overlaps)
                    {
                        if (overlap.tag == "FootPrints")
                        {
                            check = true;
                            nbOfValidGoals++;
                            break ;
                        }
                    }
                    pointGoal.updateState(check);
                }
                if (nbOfValidGoals >= ((float)subGoal.points.Count * 0.8f))
                {
                    subGoal.valid = true;
                    nbOfValidSubGoals++;
                }
                else
                    subGoal.valid = false;
                Debug.Log(nbOfValidGoals);
            }
            if (nbOfValidSubGoals == m_subGoals.Count)
            {
                GameManager.Instance.win();
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.name);
            m_subGoals.Add(new SubGoal(child));
        }

        StartCoroutine("updateGoals");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
