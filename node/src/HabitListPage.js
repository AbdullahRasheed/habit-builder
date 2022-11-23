import { useEffect, useState } from 'react';
import './HabitListPage.css';
import {Get} from './RESTRequest';
import CreateHabit from './components/CreateHabit';

function HabitListPage(){
    const [habitList, setHabitList] = useState();
    const [habitQueue, setHabitQueue] = useState();

    useEffect(() => {
        const fetchInfo = async () => {
            await Get("habit/info")
                .then(response => {
                    setHabitList(response.data.map((habit, i) => {
                        return <HabitListItem key={i} {...habit} />
                    }));
                });
        }

        fetchInfo(); 
    }, [habitQueue]);

    function consume(response){
        setHabitQueue(prev => prev+1);
    }

    return (
        <div>
            <div>
                <CreateHabit onResponse={consume}/>
            </div>
            <HabitList habitList={habitList}/>
        </div>
    )
}

function HabitList(properties){
    if(properties.habitList === undefined) return <p>Loading...</p>

    return (
        properties.habitList
    )
}

function HabitListItem(properties){
    async function loadHabit(){
        // use properties.id to redirect
        // to the HabitPage
    }

    return (
        <div>
            <h3>{properties.name}</h3>
            <p>{properties.description}</p>
        </div>
    )
}

export default HabitListPage;