import "./NotificationList.css";

function NotificationList({list, removeNotification}) {


    return (
        <div className="notification-container">
            {list.map(n => (
                <div
                    key={n.id}
                    className={`notification notification-${n.type}`}
                    onClick={() => removeNotification(n.id)}
                >
                    {n.createdAt}: {n.message}
                </div>
            ))}
        </div>
    )
}

export default NotificationList;