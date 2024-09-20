#include <iostream>
    #include <vector>
    #include <algorithm>
    
    using namespace std;
    
    int main() 
    {
        int n;
        cin >> n;
        
        vector<int> mosquitoes(n);
        for (int i = 0; i < n; ++i)
        {
            cin >> mosquitoes[i];
        }
    
        vector<int> maxMosquitoes(n, -1);
        vector<int> prevStep(n, -1);
    
        maxMosquitoes[0] = mosquitoes[0];
    
        for (int i = 0; i < n; ++i) {
            if (maxMosquitoes[i] == -1) continue;
    
            if (i + 2 < n && maxMosquitoes[i] + mosquitoes[i + 2] > maxMosquitoes[i + 2]) 
            {
                maxMosquitoes[i + 2] = maxMosquitoes[i] + mosquitoes[i + 2];
                prevStep[i + 2] = i;
            }
    
            if (i + 3 < n && maxMosquitoes[i] + mosquitoes[i + 3] > maxMosquitoes[i + 3]) 
            {
                maxMosquitoes[i + 3] = maxMosquitoes[i] + mosquitoes[i + 3];
                prevStep[i + 3] = i;
            }
        }
    
        if (maxMosquitoes[n-1] == -1)
        {
            cout << -1 << endl;
            return 0;
        }
    
        vector<int> path;
        for (int i = n - 1; i != -1; i = prevStep[i]) 
        {
            path.push_back(i + 1);
        }
        reverse(path.begin(), path.end());
    
        cout << maxMosquitoes[n-1] << endl;
        for (int i = 0; i < path.size(); ++i)
        {
            cout << path[i];
            if (i < path.size() - 1)
            {
                cout << " ";
            }
        }
        cout << endl;
    
        return 0;
    }