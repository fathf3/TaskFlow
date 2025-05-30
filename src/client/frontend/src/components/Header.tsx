import React, { useState, useEffect } from 'react';
import { Menu, Bell, User, Search } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { apiService, Project } from '../services/api';

interface HeaderProps {
  onMenuClick: () => void;
  sidebarOpen: boolean;
}

export const Header: React.FC<HeaderProps> = ({ onMenuClick, sidebarOpen }) => {
  // Kullanıcı bilgisini localStorage'dan al
  const userStr = localStorage.getItem('user');
  const user = userStr ? JSON.parse(userStr) : null;

  const fullName = user ? `${user.firstName} ${user.lastName}` : 'Kullanıcı';
  const role = user ? user.role : '';

  // Arama için state
  const [search, setSearch] = useState('');
  const [projects, setProjects] = useState<Project[]>([]);
  const [filtered, setFiltered] = useState<Project[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);

  useEffect(() => {
    apiService.getProjects().then(setProjects);
  }, []);

  useEffect(() => {
    if (search.length > 0) {
      setFiltered(
        projects.filter(p =>
          p.name.toLowerCase().includes(search.toLowerCase())
        )
      );
      setShowDropdown(true);
    } else {
      setShowDropdown(false);
    }
  }, [search, projects]);

  return (
    <header className="fixed top-0 left-0 right-0 bg-white/80 backdrop-blur-md border-b border-slate-200 z-50">
      <div className="flex items-center justify-between px-6 h-16">
        <div className="flex items-center space-x-4">
          <Button 
            variant="ghost" 
            size="sm"
            onClick={onMenuClick}
            className="hover:bg-blue-50"
          >
            <Menu className="h-5 w-5" />
          </Button>
          
          <div className="flex items-center space-x-2">
            <div className="w-8 h-8 bg-gradient-to-r from-blue-600 to-purple-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-sm">TF</span>
            </div>
            <h1 className="text-xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
              TaskFlow
            </h1>
          </div>
        </div>

        <div className="flex-1 max-w-md mx-8 relative">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-slate-400" />
            <Input
              placeholder="Projelerde ara..."
              className="pl-10 bg-slate-50 border-slate-200 focus:bg-white transition-colors"
              value={search}
              onChange={e => setSearch(e.target.value)}
              onFocus={() => search && setShowDropdown(true)}
              onBlur={() => setTimeout(() => setShowDropdown(false), 150)}
            />
          </div>
          {showDropdown && filtered.length > 0 && (
            <div className="absolute left-0 right-0 mt-2 bg-white border rounded shadow z-50 max-h-60 overflow-y-auto">
              {filtered.map(project => (
                <div
                  key={project.id}
                  className="px-4 py-2 hover:bg-blue-50 cursor-pointer"
                  onMouseDown={() => {
                    // Proje detayına yönlendirme yapılabilir
                    window.location.href = `/projects/${project.id}`;
                  }}
                >
                  {project.name}
                </div>
              ))}
            </div>
          )}
        </div>

        <div className="flex items-center space-x-4">
          <Button variant="ghost" size="sm" className="relative hover:bg-blue-50">
            <Bell className="h-5 w-5" />
            <span className="absolute -top-1 -right-1 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
              3
            </span>
          </Button>
          
          <div className="flex items-center space-x-2 cursor-pointer hover:bg-slate-50 rounded-lg p-2 transition-colors">
            <div className="w-8 h-8 bg-gradient-to-r from-green-400 to-blue-500 rounded-full flex items-center justify-center">
              <User className="h-4 w-4 text-white" />
            </div>
            <div className="hidden md:block">
              <p className="text-sm font-medium text-slate-700">{fullName}</p>
              <p className="text-xs text-slate-500">{role}</p>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};
