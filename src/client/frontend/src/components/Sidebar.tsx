import React from 'react';
import { 
  LayoutDashboard, 
  FolderOpen, 
  CheckSquare, 
  Users, 
  Settings,
  Plus
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Link } from 'react-router-dom';

interface SidebarProps {
  isOpen: boolean;
  activeView: string;
  onViewChange: (view: string) => void;
}

const menuItems = [
  { id: 'dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { id: 'projects', label: 'Projects', icon: FolderOpen },
  { id: 'tasks', label: 'Tasks', icon: CheckSquare },
  { id: 'users', label: 'Team', icon: Users },
  { id: 'settings', label: 'Settings', icon: Settings },
  { id: 'login', label: 'Giriş Yap', icon: Settings, link: '/login' }, // link eklendi
];

export const Sidebar: React.FC<SidebarProps> = ({ isOpen, activeView, onViewChange }) => {
  return (
    <aside className={`fixed left-0 top-16 h-full bg-white border-r border-slate-200 transition-all duration-300 z-40 ${
      isOpen ? 'w-64' : 'w-16'
    }`}>
      <div className="p-4">
        {isOpen && (
          <Button className="w-full bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 mb-6">
            <Plus className="h-4 w-4 mr-2" />
            New Project
          </Button>
        )}
        
        <nav className="space-y-2">
          {menuItems.map((item) =>
            item.link ? (
              <Link
                key={item.id}
                to={item.link}
                className={`w-full flex items-center space-x-3 px-3 py-3 rounded-lg transition-all duration-200 ${
                  activeView === item.id
                    ? 'bg-gradient-to-r from-blue-50 to-purple-50 text-blue-600 border-l-4 border-blue-600'
                    : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
                }`}
              >
                <item.icon className={`h-5 w-5 ${!isOpen ? 'mx-auto' : ''}`} />
                {isOpen && <span className="font-medium">{item.label}</span>}
              </Link>
            ) : (
              <button
                key={item.id}
                onClick={() => onViewChange(item.id)}
                className={`w-full flex items-center space-x-3 px-3 py-3 rounded-lg transition-all duration-200 ${
                  activeView === item.id
                    ? 'bg-gradient-to-r from-blue-50 to-purple-50 text-blue-600 border-l-4 border-blue-600'
                    : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
                }`}
              >
                <item.icon className={`h-5 w-5 ${!isOpen ? 'mx-auto' : ''}`} />
                {isOpen && <span className="font-medium">{item.label}</span>}
              </button>
            )
          )}
        </nav>
      </div>
    </aside>
  );
};
